using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create.BigovkaMarks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JobSpace.Static.Pdf.Visual.BigovkaMarks
{
    public static class Bigovka3DHtmlExporter
    {
        private const int TextureDpi = 110;

        public static string ExportAndOpen(string pdfPath, double pageWidth, double pageHeight, CreateBigovkaMarksParams parameters)
        {
            string htmlPath = Export(pdfPath, pageWidth, pageHeight, parameters);

            Process.Start(new ProcessStartInfo
            {
                FileName = htmlPath,
                UseShellExecute = true
            });

            return htmlPath;
        }

        public static string Export(string pdfPath, double pageWidth, double pageHeight, CreateBigovkaMarksParams parameters)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
                throw new ArgumentNullException(nameof(pdfPath));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (parameters.Bigovki == null || parameters.Bigovki.Length == 0)
                throw new ArgumentException("At least one bigovka mark is required.", nameof(parameters));

            double[] creases = CreateCreases(parameters, pageWidth, pageHeight);
            if (creases.Length == 0)
                throw new ArgumentException("No bigovka mark fits inside the page.", nameof(parameters));

            string front = RenderPageDataUri(pdfPath, 0, null);
            string back = RenderPageDataUri(pdfPath, 1, null);

            string html = BuildHtml(
                Path.GetFileName(pdfPath),
                pageWidth,
                pageHeight,
                creases,
                parameters.Direction,
                parameters.MirrorEven,
                front,
                back);

            string directory = Path.GetDirectoryName(pdfPath);
            string baseName = Path.GetFileNameWithoutExtension(pdfPath);
            string htmlPath = Path.Combine(directory, $"{baseName}_3d_bigovka.html");

            File.WriteAllText(htmlPath, html, new UTF8Encoding(false));
            return htmlPath;
        }

        private static double[] CreateCreases(CreateBigovkaMarksParams parameters, double pageWidth, double pageHeight)
        {
            double limit = parameters.Direction == DirectionEnum.Horizontal ? pageWidth : pageHeight;
            double current = 0;
            List<double> creases = new List<double>();

            foreach (double distance in parameters.Bigovki.Where(x => x > 0))
            {
                current += distance;
                if (current > 0 && current < limit)
                    creases.Add(current);
            }

            return creases.Distinct().OrderBy(x => x).ToArray();
        }

        private static string RenderPageDataUri(string pdfPath, int pageIndex, string fallback)
        {
            int pageCount = PdfHelper.GetPageCount(pdfPath);
            if (pageIndex >= pageCount)
                return fallback ?? CreateBlankDataUri();

            using (Bitmap bitmap = PdfHelper.RenderByTrimBox(pdfPath, pageIndex, TextureDpi))
            using (Bitmap normalized = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb))
            using (MemoryStream stream = new MemoryStream())
            {
                using (Graphics g = Graphics.FromImage(normalized))
                {
                    g.Clear(Color.White);
                    g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                }

                normalized.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + System.Convert.ToBase64String(stream.ToArray());
            }
        }

        private static string CreateBlankDataUri()
        {
            using (Bitmap bitmap = new Bitmap(16, 16))
            using (Graphics g = Graphics.FromImage(bitmap))
            using (MemoryStream stream = new MemoryStream())
            {
                g.Clear(Color.White);
                bitmap.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + System.Convert.ToBase64String(stream.ToArray());
            }
        }

        private static string BuildHtml(string title, double pageWidth, double pageHeight, double[] creases, DirectionEnum direction, bool mirrored, string frontDataUri, string backDataUri)
        {
            string html = @"<!doctype html>
<html lang=""uk"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width,initial-scale=1"">
<title>3D Bigovka - __TITLE_HTML__</title>
<style>
html,body{margin:0;height:100%;overflow:hidden;background:#202329;color:#f1f3f5;font-family:Segoe UI,Arial,sans-serif}
#stage{position:fixed;inset:0;width:100%;height:100%;display:block}
.panel{position:fixed;left:14px;top:14px;display:flex;gap:12px;align-items:center;background:rgba(20,22,26,.82);border:1px solid rgba(255,255,255,.12);border-radius:8px;padding:10px 12px;backdrop-filter:blur(8px);box-shadow:0 12px 30px rgba(0,0,0,.25)}
.panel label{display:flex;gap:8px;align-items:center;font-size:13px;white-space:nowrap}
.panel input[type=checkbox]{margin:0}
.panel input[type=range]{width:220px}
.panel select{background:#2c3038;color:#fff;border:1px solid rgba(255,255,255,.18);border-radius:5px;padding:4px 8px}
.value{min-width:72px;text-align:right;font-variant-numeric:tabular-nums}
.hint{position:fixed;left:16px;bottom:12px;color:rgba(255,255,255,.62);font-size:12px}
@media(max-width:720px){.panel{right:14px;align-items:flex-start;flex-direction:column}.panel input[type=range]{width:calc(100vw - 150px)}}
</style>
</head>
<body>
<canvas id=""stage""></canvas>
<div class=""panel"">
  <label>Згин <input id=""fold"" type=""range"" min=""0"" max=""180"" value=""0"" step=""1""><span id=""foldValue"" class=""value"">0°</span></label>
  <label>Схема <select id=""mode""><option value=""same"">в один бік</option><option value=""accordion"">гармошка</option></select></label>
  <label><input id=""showCreases"" type=""checkbox"" checked> показати біговки</label>
</div>
<div class=""hint"">Миша: обертати. Колесо: масштаб. Подвійний клік: скинути вигляд.</div>
<script>
'use strict';
const config = {
  title: __TITLE_JS__,
  width: __WIDTH__,
  height: __HEIGHT__,
  direction: __DIRECTION__,
  mirrored: __MIRRORED__,
  creases: __CREASES__,
  front: __FRONT__,
  back: __BACK__
};

const foldSettings = {
  bendDirection: -1,
  arcRadius: 0.08,
  arcRadiusStep: 0.18,
  paperHalfThickness: 0.01,
  foldedLayerGap: 0.02
};

const canvas = document.getElementById('stage');
const gl = canvas.getContext('webgl', { antialias: true, alpha: false });
if (!gl) throw new Error('WebGL is not supported.');

const vs = `
attribute vec3 aPosition;
attribute vec2 aUv;
uniform mat4 uMatrix;
varying vec2 vUv;
varying float vDepth;
void main() {
  vec4 p = uMatrix * vec4(aPosition, 1.0);
  gl_Position = p;
  vUv = aUv;
  vDepth = clamp((aPosition.z + 180.0) / 360.0, 0.0, 1.0);
}`;
const fs = `
precision mediump float;
uniform sampler2D uTexture;
uniform float uBack;
varying vec2 vUv;
varying float vDepth;
void main() {
  vec4 c = texture2D(uTexture, vUv);
  float shade = mix(0.86, 1.06, vDepth);
  if (uBack > 0.5) shade *= 0.94;
  gl_FragColor = vec4(c.rgb * shade, 1.0);
}`;
const lineVs = `
attribute vec3 aPosition;
uniform mat4 uMatrix;
void main() {
  gl_Position = uMatrix * vec4(aPosition, 1.0);
}`;
const lineFs = `
precision mediump float;
uniform vec4 uColor;
void main() {
  gl_FragColor = uColor;
}`;

function compile(type, source) {
  const shader = gl.createShader(type);
  gl.shaderSource(shader, source);
  gl.compileShader(shader);
  if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) throw new Error(gl.getShaderInfoLog(shader));
  return shader;
}
const program = gl.createProgram();
gl.attachShader(program, compile(gl.VERTEX_SHADER, vs));
gl.attachShader(program, compile(gl.FRAGMENT_SHADER, fs));
gl.linkProgram(program);
if (!gl.getProgramParameter(program, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(program));
gl.useProgram(program);

const loc = {
  position: gl.getAttribLocation(program, 'aPosition'),
  uv: gl.getAttribLocation(program, 'aUv'),
  matrix: gl.getUniformLocation(program, 'uMatrix'),
  texture: gl.getUniformLocation(program, 'uTexture'),
  back: gl.getUniformLocation(program, 'uBack')
};

const lineProgram = gl.createProgram();
gl.attachShader(lineProgram, compile(gl.VERTEX_SHADER, lineVs));
gl.attachShader(lineProgram, compile(gl.FRAGMENT_SHADER, lineFs));
gl.linkProgram(lineProgram);
if (!gl.getProgramParameter(lineProgram, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(lineProgram));
const lineLoc = {
  position: gl.getAttribLocation(lineProgram, 'aPosition'),
  matrix: gl.getUniformLocation(lineProgram, 'uMatrix'),
  color: gl.getUniformLocation(lineProgram, 'uColor')
};

const posBuffer = gl.createBuffer();
const uvBuffer = gl.createBuffer();
const indexBuffer = gl.createBuffer();
const creaseLineBuffer = gl.createBuffer();
const foldSlider = document.getElementById('fold');
const foldValue = document.getElementById('foldValue');
const modeSelect = document.getElementById('mode');
const showCreasesCheckbox = document.getElementById('showCreases');

const state = {
  rotX: -0.58,
  rotY: 0.62,
  zoom: 1.15,
  dragging: false,
  lastX: 0,
  lastY: 0
};

function loadTexture(src) {
  const tex = gl.createTexture();
  gl.bindTexture(gl.TEXTURE_2D, tex);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
  gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, 1, 1, 0, gl.RGBA, gl.UNSIGNED_BYTE, new Uint8Array([255,255,255,255]));
  const img = new Image();
  img.onload = () => {
    gl.bindTexture(gl.TEXTURE_2D, tex);
    gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img);
    requestAnimationFrame(draw);
  };
  img.src = src;
  return tex;
}

const textures = {
  front: loadTexture(config.front),
  back: loadTexture(config.back)
};

const xs = buildSamples('x');
const ys = buildSamples('y');
const indices = buildIndices(xs.length, ys.length);
const uvsFront = buildUvs(false);
const uvsBack = buildUvs(true);

function buildSamples(axis) {
  const size = axis === 'x' ? config.width : config.height;
  const creases = axisMatchesDirection(axis) ? config.creases : [];
  const step = Math.max(2, size / 150);
  const set = new Set();
  for (let v = 0; v <= size + 0.01; v += step) set.add(round3(Math.min(size, v)));
  set.add(0); set.add(round3(size));
  for (let creaseIndex = 0; creaseIndex < creases.length; creaseIndex++) {
    const f = creases[creaseIndex];
    const r = foldRadius(creaseIndex);
    const maxArc = Math.PI * r;
    for (let i = 0; i <= 24; i++) {
      set.add(round3(clamp(f - maxArc * 0.5 * i / 24, 0, size)));
      set.add(round3(clamp(f + maxArc * 0.5 * i / 24, 0, size)));
    }
  }
  return Array.from(set).sort((a,b) => a-b);
}

function axisMatchesDirection(axis) {
  return (config.direction === 'horizontal' && axis === 'x') || (config.direction === 'vertical' && axis === 'y');
}

function buildIndices(nx, ny) {
  const out = [];
  for (let y = 0; y < ny - 1; y++) {
    for (let x = 0; x < nx - 1; x++) {
      const a = y * nx + x;
      const b = a + 1;
      const c = a + nx;
      const d = c + 1;
      out.push(a, c, b, b, c, d);
    }
  }
  return new Uint16Array(out);
}

function buildUvs(back) {
  const arr = [];
  for (const y of ys) {
    const v = 1 - y / config.height;
    for (const x of xs) {
      const u = x / config.width;
      arr.push(back ? 1 - u : u, v);
    }
  }
  return new Float32Array(arr);
}

function foldRadius(foldIndex) {
  return foldSettings.arcRadius + foldSettings.arcRadiusStep * foldIndex;
}

function foldAnglesForCurrent() {
  const angle = Number(foldSlider.value) * Math.PI / 180;
  const angles = new Array(config.creases.length);
  for (let i = 0; i < angles.length; i++) {
    const dir = modeSelect.value === 'accordion' && i % 2 === 1 ? -1 : 1;
    angles[i] = dir * angle;
  }
  return angles;
}

function transformPoint(source, foldAngles, sideZ) {
  let p = { x: source.x, y: source.y, z: sideZ };

  for (let i = config.creases.length - 1; i >= 0; i--) {
    p = applyCreaseArcFold(p, source, config.creases[i], foldAngles[i] || 0, foldRadius(i));
  }

  p.z += layerOffset(source, foldAngles);
  return p;
}

function applyCreaseArcFold(p, source, crease, angle, radius) {
  if (Math.abs(angle) < 0.0001) return p;

  const sourceValue = creaseCoordinate(source);
  const angleSign = angle < 0 ? -1 : 1;
  const absAngle = Math.abs(angle);
  const arcDirection = rollArcDirection() * angleSign;
  const arcLength = radius * absAngle;
  const leftBoundary = crease - arcLength * 0.5;
  const rightBoundary = crease + arcLength * 0.5;

  if (sourceValue <= leftBoundary) return p;

  if (sourceValue < rightBoundary) {
    const a = clamp((sourceValue - leftBoundary) / radius, 0, absAngle);
    const signedA = angleSign * a;
    const centerValue = leftBoundary + radius * Math.sin(a);
    const centerZ = arcDirection * radius * (1 - Math.cos(a));
    return setCreaseCoordinate(p, centerValue, centerZ, signedA);
  }

  const arcEnd = {
    value: leftBoundary + radius * Math.sin(absAngle),
    z: arcDirection * radius * (1 - Math.cos(absAngle))
  };
  const boundary = creaseAxisPoint(rightBoundary);
  const rotated = config.direction === 'horizontal'
    ? rotatePointY(p, boundary, angle)
    : rotatePointX(p, boundary, -angle);

  return translateAlongCreaseCoordinate(rotated, arcEnd.value - rightBoundary, arcEnd.z);
}

function rollArcDirection() {
  return foldSettings.bendDirection;
}

function layerOffset(source, foldAngles) {
  let offset = 0;
  const value = creaseCoordinate(source);
  for (let i = 0; i < config.creases.length; i++) {
    if (value > config.creases[i]) offset += foldSettings.foldedLayerGap * clamp(Math.abs(foldAngles[i] || 0) / Math.PI, 0, 1);
  }
  return offset;
}

function creaseCoordinate(source) {
  return config.direction === 'horizontal' ? source.x : source.y + config.height * 0.5;
}

function creaseAxisPoint(value) {
  return config.direction === 'horizontal'
    ? { x: value, y: 0, z: 0 }
    : { x: 0, y: value - config.height * 0.5, z: 0 };
}

function setCreaseCoordinate(p, value, z, angle) {
  if (config.direction === 'horizontal') {
    return {
      x: value + p.z * Math.sin(angle),
      y: p.y,
      z: z + p.z * Math.cos(angle)
    };
  }

  return {
    x: p.x,
    y: value - config.height * 0.5 - p.z * Math.sin(angle),
    z: z + p.z * Math.cos(angle)
  };
}

function translateAlongCreaseCoordinate(p, valueOffset, zOffset) {
  if (config.direction === 'horizontal') {
    return {
      x: p.x + valueOffset,
      y: p.y,
      z: p.z + zOffset
    };
  }

  return {
    x: p.x,
    y: p.y + valueOffset,
    z: p.z + zOffset
  };
}

function rotatePointY(p, axis, angle) {
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  const dx = p.x - axis.x;
  const dz = p.z - axis.z;
  return {
    x: axis.x + dx * c + dz * s,
    y: p.y,
    z: axis.z - dx * s + dz * c
  };
}

function rotatePointX(p, axis, angle) {
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  const dy = p.y - axis.y;
  const dz = p.z - axis.z;
  return {
    x: p.x,
    y: axis.y + dy * c - dz * s,
    z: axis.z + dy * s + dz * c
  };
}

function buildPositions(back) {
  const foldAngles = foldAnglesForCurrent();
  const sideZ = back ? -foldSettings.paperHalfThickness : foldSettings.paperHalfThickness;
  const raw = [];
  for (const y of ys) {
    const modelY = config.height * 0.5 - y;
    for (const x of xs) raw.push(transformPoint({ x, y: modelY }, foldAngles, sideZ));
  }
  const center = getModelCenter(foldAngles);
  const arr = [];
  for (const p of raw) arr.push(p.x - center.x, p.y - center.y, p.z - center.z);
  return new Float32Array(arr);
}

function getModelCenter(foldAngles) {
  const min = { x: Infinity, y: Infinity, z: Infinity };
  const max = { x: -Infinity, y: -Infinity, z: -Infinity };
  for (const y of ys) {
    const modelY = config.height * 0.5 - y;
    for (const x of xs) {
      const p = transformPoint({ x, y: modelY }, foldAngles, 0);
      min.x = Math.min(min.x, p.x); min.y = Math.min(min.y, p.y); min.z = Math.min(min.z, p.z);
      max.x = Math.max(max.x, p.x); max.y = Math.max(max.y, p.y); max.z = Math.max(max.z, p.z);
    }
  }
  return { x: (min.x + max.x) * 0.5, y: (min.y + max.y) * 0.5, z: (min.z + max.z) * 0.5 };
}

function buildCreaseLinePositions() {
  if (!showCreasesCheckbox.checked) return new Float32Array(0);

  const foldAngles = foldAnglesForCurrent();
  const center = getModelCenter(foldAngles);
  const arr = [];
  const z = foldSettings.paperHalfThickness + 0.08;

  for (const crease of config.creases) {
    if (config.direction === 'horizontal') {
      addLinePoint(arr, { x: crease, y: -config.height * 0.5 }, foldAngles, z, center);
      addLinePoint(arr, { x: crease, y: config.height * 0.5 }, foldAngles, z, center);
    } else {
      addLinePoint(arr, { x: 0, y: crease - config.height * 0.5 }, foldAngles, z, center);
      addLinePoint(arr, { x: config.width, y: crease - config.height * 0.5 }, foldAngles, z, center);
    }
  }

  return new Float32Array(arr);
}

function addLinePoint(arr, point, foldAngles, z, center) {
  const p = transformPoint(point, foldAngles, z);
  arr.push(p.x - center.x, p.y - center.y, p.z - center.z);
}

function drawMesh(texture, back) {
  const positions = buildPositions(back);
  gl.bindBuffer(gl.ARRAY_BUFFER, posBuffer);
  gl.bufferData(gl.ARRAY_BUFFER, positions, gl.DYNAMIC_DRAW);
  gl.enableVertexAttribArray(loc.position);
  gl.vertexAttribPointer(loc.position, 3, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ARRAY_BUFFER, uvBuffer);
  gl.bufferData(gl.ARRAY_BUFFER, back ? uvsBack : uvsFront, gl.STATIC_DRAW);
  gl.enableVertexAttribArray(loc.uv);
  gl.vertexAttribPointer(loc.uv, 2, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, indexBuffer);
  gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, indices, gl.STATIC_DRAW);

  gl.activeTexture(gl.TEXTURE0);
  gl.bindTexture(gl.TEXTURE_2D, texture);
  gl.uniform1i(loc.texture, 0);
  gl.uniform1f(loc.back, back ? 1 : 0);
  gl.enable(gl.CULL_FACE);
  gl.frontFace(gl.CCW);
  gl.cullFace(back ? gl.FRONT : gl.BACK);
  gl.enable(gl.POLYGON_OFFSET_FILL);
  gl.polygonOffset(back ? 1 : -1, back ? 1 : -1);
  gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);
  gl.disable(gl.POLYGON_OFFSET_FILL);
  gl.disable(gl.CULL_FACE);
}

function drawCreaseLines(matrix) {
  const positions = buildCreaseLinePositions();
  if (!positions.length) return;

  gl.useProgram(lineProgram);
  gl.bindBuffer(gl.ARRAY_BUFFER, creaseLineBuffer);
  gl.bufferData(gl.ARRAY_BUFFER, positions, gl.DYNAMIC_DRAW);
  gl.enableVertexAttribArray(lineLoc.position);
  gl.vertexAttribPointer(lineLoc.position, 3, gl.FLOAT, false, 0, 0);
  gl.uniformMatrix4fv(lineLoc.matrix, false, matrix);
  gl.uniform4f(lineLoc.color, 0.1, 0.95, 0.45, 1.0);
  gl.disable(gl.CULL_FACE);
  gl.enable(gl.DEPTH_TEST);
  gl.depthMask(false);
  gl.lineWidth(2);
  gl.drawArrays(gl.LINES, 0, positions.length / 3);
  gl.depthMask(true);
  gl.useProgram(program);
}

function draw() {
  resize();
  foldValue.textContent = foldSlider.value + '°';
  gl.viewport(0, 0, canvas.width, canvas.height);
  gl.clearColor(0.125, 0.137, 0.16, 1);
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
  gl.enable(gl.DEPTH_TEST);

  const aspect = canvas.width / Math.max(1, canvas.height);
  const maxDim = Math.max(config.width, config.height);
  const projection = perspective(42 * Math.PI / 180, aspect, 1, maxDim * 10);
  let view = identity();
  view = multiply(view, translate(0, 0, -maxDim * 2.15 / state.zoom));
  view = multiply(view, rotateX(state.rotX));
  view = multiply(view, rotateMatrixY(state.rotY));
  const matrix = multiply(projection, view);
  gl.useProgram(program);
  gl.uniformMatrix4fv(loc.matrix, false, matrix);

  drawMesh(textures.back, true);
  drawMesh(textures.front, false);
  drawCreaseLines(matrix);
}

function resize() {
  const dpr = Math.min(window.devicePixelRatio || 1, 2);
  const w = Math.floor(canvas.clientWidth * dpr);
  const h = Math.floor(canvas.clientHeight * dpr);
  if (canvas.width !== w || canvas.height !== h) {
    canvas.width = w;
    canvas.height = h;
  }
}

function perspective(fov, aspect, near, far) {
  const f = 1 / Math.tan(fov / 2);
  const nf = 1 / (near - far);
  return new Float32Array([
    f / aspect, 0, 0, 0,
    0, f, 0, 0,
    0, 0, (far + near) * nf, -1,
    0, 0, (2 * far * near) * nf, 0
  ]);
}
function identity() {
  return new Float32Array([1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1]);
}
function multiply(a, b) {
  const o = new Float32Array(16);
  for (let r = 0; r < 4; r++) {
    for (let c = 0; c < 4; c++) {
      o[c * 4 + r] = a[0 * 4 + r] * b[c * 4 + 0] + a[1 * 4 + r] * b[c * 4 + 1] + a[2 * 4 + r] * b[c * 4 + 2] + a[3 * 4 + r] * b[c * 4 + 3];
    }
  }
  return o;
}
function translate(x, y, z) {
  const m = identity();
  m[12] = x; m[13] = y; m[14] = z;
  return m;
}
function rotateX(a) {
  const s = Math.sin(a), c = Math.cos(a);
  return new Float32Array([1,0,0,0, 0,c,s,0, 0,-s,c,0, 0,0,0,1]);
}
function rotateMatrixY(a) {
  const s = Math.sin(a), c = Math.cos(a);
  return new Float32Array([c,0,-s,0, 0,1,0,0, s,0,c,0, 0,0,0,1]);
}
function smoothstep(t) {
  t = clamp(t, 0, 1);
  return t * t * (3 - 2 * t);
}
function clamp(v, min, max) { return Math.max(min, Math.min(max, v)); }
function round3(v) { return Math.round(v * 1000) / 1000; }

canvas.addEventListener('pointerdown', e => {
  state.dragging = true;
  state.lastX = e.clientX;
  state.lastY = e.clientY;
  canvas.setPointerCapture(e.pointerId);
});
canvas.addEventListener('pointermove', e => {
  if (!state.dragging) return;
  const dx = e.clientX - state.lastX;
  const dy = e.clientY - state.lastY;
  state.lastX = e.clientX;
  state.lastY = e.clientY;
  state.rotY += dx * 0.01;
  state.rotX += dy * 0.01;
  state.rotX = clamp(state.rotX, -1.45, 1.45);
  requestAnimationFrame(draw);
});
canvas.addEventListener('pointerup', e => {
  state.dragging = false;
  canvas.releasePointerCapture(e.pointerId);
});
canvas.addEventListener('wheel', e => {
  e.preventDefault();
  state.zoom *= Math.exp(-e.deltaY * 0.001);
  state.zoom = clamp(state.zoom, 0.35, 4);
  requestAnimationFrame(draw);
}, { passive: false });
canvas.addEventListener('dblclick', () => {
  state.rotX = -0.58;
  state.rotY = 0.62;
  state.zoom = 1.15;
  requestAnimationFrame(draw);
});
foldSlider.addEventListener('input', () => requestAnimationFrame(draw));
modeSelect.addEventListener('change', () => requestAnimationFrame(draw));
showCreasesCheckbox.addEventListener('change', () => requestAnimationFrame(draw));
window.addEventListener('resize', () => requestAnimationFrame(draw));
requestAnimationFrame(draw);
</script>
</body>
</html>";

            return html
                .Replace("__TITLE_HTML__", EscapeHtml(title))
                .Replace("__TITLE_JS__", JsString(title))
                .Replace("__WIDTH__", ToJsNumber(pageWidth))
                .Replace("__HEIGHT__", ToJsNumber(pageHeight))
                .Replace("__DIRECTION__", JsString(direction == DirectionEnum.Horizontal ? "horizontal" : "vertical"))
                .Replace("__MIRRORED__", mirrored ? "true" : "false")
                .Replace("__CREASES__", "[" + string.Join(",", creases.Select(ToJsNumber)) + "]")
                .Replace("__FRONT__", JsString(frontDataUri))
                .Replace("__BACK__", JsString(backDataUri));
        }

        private static string ToJsNumber(double value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        private static string JsString(string value)
        {
            if (value == null)
                return "null";

            return "\"" + value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("<", "\\u003c")
                .Replace(">", "\\u003e")
                .Replace("&", "\\u0026") + "\"";
        }

        private static string EscapeHtml(string value)
        {
            if (value == null)
                return string.Empty;

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }
    }
}
