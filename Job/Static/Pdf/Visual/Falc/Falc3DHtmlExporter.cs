using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JobSpace.Static.Pdf.Visual.Falc
{
    public static class Falc3DHtmlExporter
    {
        private const int TextureDpi = 110;

        public static string ExportAndOpen(string pdfPath, decimal pageWidth, decimal pageHeight, decimal[] partsWidth, bool mirrored)
        {
            string htmlPath = Export(pdfPath, pageWidth, pageHeight, partsWidth, mirrored);

            Process.Start(new ProcessStartInfo
            {
                FileName = htmlPath,
                UseShellExecute = true
            });

            return htmlPath;
        }

        public static string Export(string pdfPath, decimal pageWidth, decimal pageHeight, decimal[] partsWidth, bool mirrored)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
                throw new ArgumentNullException(nameof(pdfPath));
            if (partsWidth == null || partsWidth.Length < 2)
                throw new ArgumentException("At least two folded parts are required.", nameof(partsWidth));

            decimal[] modelParts = partsWidth.Where(x => x > 0).ToArray();
            if (modelParts.Length < 2)
                throw new ArgumentException("At least two folded parts are required.", nameof(partsWidth));

            if (mirrored)
                modelParts = modelParts.Reverse().ToArray();

            string front = RenderPageDataUri(pdfPath, 0, null);
            string back = RenderPageDataUri(pdfPath, 1, front);

            string html = BuildHtml(
                Path.GetFileName(pdfPath),
                (double)modelParts.Sum(),
                (double)pageHeight,
                modelParts.Select(x => (double)x).ToArray(),
                front,
                back);

            string directory = Path.GetDirectoryName(pdfPath);
            string baseName = Path.GetFileNameWithoutExtension(pdfPath);
            string htmlPath = Path.Combine(directory, $"{baseName}_3d_falc.html");

            File.WriteAllText(htmlPath, html, new UTF8Encoding(false));
            return htmlPath;
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

        private static string BuildHtml(string title, double pageWidth, double pageHeight, double[] partsWidth, string frontDataUri, string backDataUri)
        {
            string jsonTitle = JsString(title);
            string jsonParts = "[" + string.Join(",", partsWidth.Select(ToJsNumber)) + "]";

            return $@"<!doctype html>
<html lang=""uk"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width,initial-scale=1"">
<title>3D Falc - {EscapeHtml(title)}</title>
<style>
html,body{{margin:0;height:100%;overflow:hidden;background:#202329;color:#f1f3f5;font-family:Segoe UI,Arial,sans-serif}}
#stage{{position:fixed;inset:0;width:100%;height:100%;display:block}}
.panel{{position:fixed;left:14px;top:14px;display:flex;gap:12px;align-items:center;background:rgba(20,22,26,.82);border:1px solid rgba(255,255,255,.12);border-radius:8px;padding:10px 12px;backdrop-filter:blur(8px);box-shadow:0 12px 30px rgba(0,0,0,.25)}}
.panel label{{display:flex;gap:8px;align-items:center;font-size:13px;white-space:nowrap}}
.panel input[type=checkbox]{{margin:0}}
.panel input[type=range]{{width:220px}}
.panel select{{background:#2c3038;color:#fff;border:1px solid rgba(255,255,255,.18);border-radius:5px;padding:4px 8px}}
.value{{min-width:72px;text-align:right;font-variant-numeric:tabular-nums}}
.hint{{position:fixed;left:16px;bottom:12px;color:rgba(255,255,255,.62);font-size:12px}}
@media(max-width:720px){{.panel{{right:14px;align-items:flex-start;flex-direction:column}}.panel input[type=range]{{width:calc(100vw - 150px)}}}}
</style>
</head>
<body>
<canvas id=""stage""></canvas>
<div class=""panel"">
  <label>Згин <input id=""fold"" type=""range"" min=""0"" max=""180"" value=""0"" step=""1""><span id=""foldValue"" class=""value"">0°</span></label>
  <label>Схема <select id=""mode""><option value=""roll"">намотка</option><option value=""accordion"">гармошка</option></select></label>
  <label><input id=""showFoldLines"" type=""checkbox""> показати лінії фальцювання</label>
</div>
<div class=""hint"">Миша: обертати. Колесо: масштаб. Подвійний клік: скинути вигляд.</div>
<script>
'use strict';
const config = {{
  title: {jsonTitle},
  width: {ToJsNumber(pageWidth)},
  height: {ToJsNumber(pageHeight)},
  parts: {jsonParts},
  front: {JsString(frontDataUri)},
  back: {JsString(backDataUri)}
}};

const foldSettings = {{
  // Напрямок згину/дуги фальця: 1 або -1. Міняє, в який бік випинається U-подібна дуга.
  bendDirection: -1,

  // Радіус дуги фальця в одиницях моделі (тут це приблизно міліметри PDF). Більше значення = ширший округлий згин.
  arcRadius: 0.08,

  // Додаток до радіуса для кожного наступного фальця: 1-й = arcRadius, 2-й = arcRadius + arcRadiusStep, 3-й = arcRadius + arcRadiusStep * 2.
  arcRadiusStep: 0.2,

  // Половина відстані між лицем і зворотом. Загальна візуальна товщина паперу = paperHalfThickness * 2.
  paperHalfThickness: 0.01,

  // Мінімальний проміжок між уже складеними шарами, щоб поверхні не перетинались і не мерехтіли.
  foldedLayerGap: 0.02
}};

const canvas = document.getElementById('stage');
const gl = canvas.getContext('webgl', {{ antialias: true, alpha: false }});
if (!gl) throw new Error('WebGL is not supported.');

const vs = `
attribute vec3 aPosition;
attribute vec2 aUv;
uniform mat4 uMatrix;
varying vec2 vUv;
varying float vDepth;
void main() {{
  vec4 p = uMatrix * vec4(aPosition, 1.0);
  gl_Position = p;
  vUv = aUv;
  vDepth = clamp((aPosition.z + 180.0) / 360.0, 0.0, 1.0);
}}`;
const fs = `
precision mediump float;
uniform sampler2D uTexture;
uniform float uBack;
varying vec2 vUv;
varying float vDepth;
void main() {{
  vec4 c = texture2D(uTexture, vUv);
  float shade = mix(0.86, 1.06, vDepth);
  if (uBack > 0.5) shade *= 0.94;
  gl_FragColor = vec4(c.rgb * shade, 1.0);
}}`;
const lineVs = `
attribute vec3 aPosition;
uniform mat4 uMatrix;
void main() {{
  gl_Position = uMatrix * vec4(aPosition, 1.0);
}}`;
const lineFs = `
precision mediump float;
uniform vec4 uColor;
void main() {{
  gl_FragColor = uColor;
}}`;

function compile(type, source) {{
  const shader = gl.createShader(type);
  gl.shaderSource(shader, source);
  gl.compileShader(shader);
  if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) throw new Error(gl.getShaderInfoLog(shader));
  return shader;
}}
const program = gl.createProgram();
gl.attachShader(program, compile(gl.VERTEX_SHADER, vs));
gl.attachShader(program, compile(gl.FRAGMENT_SHADER, fs));
gl.linkProgram(program);
if (!gl.getProgramParameter(program, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(program));
gl.useProgram(program);

const loc = {{
  position: gl.getAttribLocation(program, 'aPosition'),
  uv: gl.getAttribLocation(program, 'aUv'),
  matrix: gl.getUniformLocation(program, 'uMatrix'),
  texture: gl.getUniformLocation(program, 'uTexture'),
  back: gl.getUniformLocation(program, 'uBack')
}};

const lineProgram = gl.createProgram();
gl.attachShader(lineProgram, compile(gl.VERTEX_SHADER, lineVs));
gl.attachShader(lineProgram, compile(gl.FRAGMENT_SHADER, lineFs));
gl.linkProgram(lineProgram);
if (!gl.getProgramParameter(lineProgram, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(lineProgram));

const lineLoc = {{
  position: gl.getAttribLocation(lineProgram, 'aPosition'),
  matrix: gl.getUniformLocation(lineProgram, 'uMatrix'),
  color: gl.getUniformLocation(lineProgram, 'uColor')
}};

const posBuffer = gl.createBuffer();
const uvBuffer = gl.createBuffer();
const indexBuffer = gl.createBuffer();
const foldLineBuffer = gl.createBuffer();
const foldSlider = document.getElementById('fold');
const foldValue = document.getElementById('foldValue');
const modeSelect = document.getElementById('mode');
const showFoldLinesCheckbox = document.getElementById('showFoldLines');

const state = {{
  rotX: -0.58,
  rotY: 0.62,
  zoom: 1.15,
  dragging: false,
  lastX: 0,
  lastY: 0
}};

function loadTexture(src) {{
  const tex = gl.createTexture();
  gl.bindTexture(gl.TEXTURE_2D, tex);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
  gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, 1, 1, 0, gl.RGBA, gl.UNSIGNED_BYTE, new Uint8Array([255,255,255,255]));
  const img = new Image();
  img.onload = () => {{
    gl.bindTexture(gl.TEXTURE_2D, tex);
    gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img);
    requestAnimationFrame(draw);
  }};
  img.src = src;
  return tex;
}}

const textures = {{
  front: loadTexture(config.front),
  back: loadTexture(config.back)
}};

let xs = buildXSamples();
let indices = buildIndices(xs.length, 20);
let uvsFront = buildUvs(false);
let uvsBack = buildUvs(true);
updateSliderLimit();

function buildXSamples() {{
  const folds = [];
  let x = 0;
  for (let i = 0; i < config.parts.length - 1; i++) {{
    x += config.parts[i];
    folds.push(x);
  }}
  const step = Math.max(2, config.width / 160);
  const set = new Set();
  for (let v = 0; v <= config.width + 0.01; v += step) set.add(round3(Math.min(config.width, v)));
  set.add(0); set.add(round3(config.width));
  for (let foldIndex = 0; foldIndex < folds.length; foldIndex++) {{
    const f = folds[foldIndex];
    const r = foldRadius(foldIndex);
    const maxArc = Math.PI * r;
    for (let i = 0; i <= 28; i++) {{
      set.add(round3(clamp(f - maxArc * 0.5 * i / 28, 0, config.width)));
      set.add(round3(clamp(f + maxArc * 0.5 * i / 28, 0, config.width)));
    }}
  }}
  return Array.from(set).sort((a,b) => a-b);
}}

function buildIndices(nx, ny) {{
  const out = [];
  for (let y = 0; y < ny; y++) {{
    for (let x = 0; x < nx - 1; x++) {{
      const a = y * nx + x;
      const b = a + 1;
      const c = a + nx;
      const d = c + 1;
      out.push(a, c, b, b, c, d);
    }}
  }}
  return new Uint16Array(out);
}}

function buildUvs(back) {{
  const ny = 20;
  const arr = [];
  for (let iy = 0; iy <= ny; iy++) {{
    const v = iy / ny;
    for (const x of xs) {{
      const u = x / config.width;
      arr.push(back ? 1 - u : u, 1 - v);
    }}
  }}
  return new Float32Array(arr);
}}

function foldRadius(foldIndex) {{
  return foldSettings.arcRadius + foldSettings.arcRadiusStep * foldIndex;
}}

function paperHalfThickness() {{
  return foldSettings.paperHalfThickness;
}}

function foldedLayerGap() {{
  return foldSettings.foldedLayerGap;
}}

function foldPositions() {{
  const folds = [];
  let x = 0;
  for (let i = 0; i < config.parts.length - 1; i++) {{
    x += config.parts[i];
    folds.push(x);
  }}
  return folds;
}}

function foldAnglesForCurrent(folds, mode) {{
  const angles = new Array(folds.length).fill(0);
  const slider = Number(foldSlider.value);
  if (mode === 'roll') {{
    const order = rollFoldOrder(folds);
    for (let i = 0; i < order.length; i++) {{
      angles[order[i]] = clamp(slider - i * 180, 0, 180) * Math.PI / 180;
    }}
    return angles;
  }}

  const angle = slider * Math.PI / 180;
  for (let i = 0; i < folds.length; i++) angles[i] = angle;
  return angles;
}}

function rollFoldOrder(folds) {{
  const order = [];
  for (let i = 0; i < folds.length; i++) order.push(i);
  return order;
}}

function updateSliderLimit() {{
  const max = modeSelect.value === 'roll'
    ? Math.max(180, (config.parts.length - 1) * 180)
    : 180;
  foldSlider.max = String(max);
  if (Number(foldSlider.value) > max) foldSlider.value = String(max);
}}


function transformPoint(source, foldAngles, folds, mode, sideZ) {{
  let p = {{ x: source.x, y: source.y, z: sideZ }};
  const radius = foldRadius(0);

  if (mode === 'roll') {{
    p.z += rollLayerOffset(source.x, foldAngles, folds);
    for (let i = 0; i < folds.length; i++) {{
      p = applyRollArcFold(p, source.x, folds[i], foldAngles[i] || 0, foldRadius(i));
    }}
    return p;
  }}

  for (let i = 0; i < folds.length; i++) {{
    const f = folds[i];
    if (source.x <= f - radius) continue;
    const angle = foldAngles[i] || 0;
    if (Math.abs(angle) < 0.0001) continue;
    const dir = mode === 'accordion' && (i % 2 === 1) ? -1 : 1;
    const factor = source.x >= f + radius ? 1 : smoothstep((source.x - (f - radius)) / (2 * radius));
    const axis = transformAxis(f, i, foldAngles, folds, mode, radius);
    p = rotatePointY(p, axis, dir * angle * factor);
  }}
  return p;
}}

function transformAxis(x, foldIndex, foldAngles, folds, mode, radius) {{
  let p = {{ x, y: 0, z: 0 }};
  for (let i = 0; i < foldIndex; i++) {{
    const f = folds[i];
    if (x <= f - radius) continue;
    const angle = foldAngles[i] || 0;
    if (Math.abs(angle) < 0.0001) continue;
    const dir = mode === 'accordion' && (i % 2 === 1) ? -1 : 1;
    const factor = x >= f + radius ? 1 : smoothstep((x - (f - radius)) / (2 * radius));
    const axis = transformAxis(f, i, foldAngles, folds, mode, radius);
    p = rotatePointY(p, axis, dir * angle * factor);
  }}
  return p;
}}

function applyRollArcFold(p, sourceX, foldX, angle, radius) {{
  if (Math.abs(angle) < 0.0001) return p;

  const arcDirection = rollArcDirection();
  const arcLength = radius * angle;
  const leftBoundary = foldX - arcLength * 0.5;
  const rightBoundary = foldX + arcLength * 0.5;

  if (sourceX >= rightBoundary) return p;

  if (sourceX > leftBoundary) {{
    const a = clamp((rightBoundary - sourceX) / radius, 0, angle);
    const centerX = rightBoundary - radius * Math.sin(a);
    const centerZ = arcDirection * radius * (1 - Math.cos(a));
    return {{
      x: centerX + arcDirection * p.z * Math.sin(a),
      y: p.y,
      z: centerZ + p.z * Math.cos(a)
    }};
  }}

  const arcEnd = {{
    x: rightBoundary - radius * Math.sin(angle),
    y: 0,
    z: arcDirection * radius * (1 - Math.cos(angle))
  }};
  const sourceBoundary = {{ x: leftBoundary, y: 0, z: 0 }};
  const rotated = rotatePointY(p, sourceBoundary, arcDirection * angle);

  return {{
    x: rotated.x + arcEnd.x - sourceBoundary.x,
    y: rotated.y,
    z: rotated.z + arcEnd.z - sourceBoundary.z
  }};
}}

function rollArcDirection() {{
  return foldSettings.bendDirection;
}}

function rollLayerOffset(x, foldAngles, folds) {{
  let offset = 0;
  const layerStep = foldedLayerGap();
  for (let i = 0; i < folds.length; i++) {{
    if (x < folds[i]) offset += layerStep * clamp((foldAngles[i] || 0) / Math.PI, 0, 1);
  }}
  return offset;
}}

function rotatePointY(p, axis, angle) {{
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  const dx = p.x - axis.x;
  const dz = p.z - axis.z;
  return {{
    x: axis.x + dx * c + dz * s,
    y: p.y,
    z: axis.z - dx * s + dz * c
  }};
}}

function buildPositions(back) {{
  const folds = foldPositions();
  const mode = modeSelect.value;
  const foldAngles = foldAnglesForCurrent(folds, mode);
  const ny = 20;
  const sideZ = back ? -paperHalfThickness() : paperHalfThickness();
  const raw = [];
  for (let iy = 0; iy <= ny; iy++) {{
    const y = config.height * (0.5 - iy / ny);
    for (const x of xs) {{
      const p = transformPoint({{ x, y }}, foldAngles, folds, mode, sideZ);
      raw.push(p);
    }}
  }}
  const center = getModelCenter(foldAngles, folds, mode);
  const arr = [];
  for (const p of raw) arr.push(p.x - center.x, p.y - center.y, p.z - center.z);
  return new Float32Array(arr);
}}

function getModelCenter(foldAngles, folds, mode) {{
  const min = {{ x: Infinity, y: Infinity, z: Infinity }};
  const max = {{ x: -Infinity, y: -Infinity, z: -Infinity }};
  const ny = 20;
  for (let iy = 0; iy <= ny; iy++) {{
    const y = config.height * (0.5 - iy / ny);
    for (const x of xs) {{
      const p = transformPoint({{ x, y }}, foldAngles, folds, mode, 0);
      min.x = Math.min(min.x, p.x); min.y = Math.min(min.y, p.y); min.z = Math.min(min.z, p.z);
      max.x = Math.max(max.x, p.x); max.y = Math.max(max.y, p.y); max.z = Math.max(max.z, p.z);
    }}
  }}
  return {{
    x: (min.x + max.x) * 0.5,
    y: (min.y + max.y) * 0.5,
    z: (min.z + max.z) * 0.5
  }};
}}

function buildFoldLinePositions() {{
  const folds = foldPositions();
  if (!folds.length) return new Float32Array(0);

  const mode = modeSelect.value;
  const foldAngles = foldAnglesForCurrent(folds, mode);
  const center = getModelCenter(foldAngles, folds, mode);
  const frontZ = paperHalfThickness() + 0.06;
  const backZ = -paperHalfThickness() - 0.06;
  const arr = [];

  for (const foldX of folds) {{
    addFoldLineSegment(arr, foldX, frontZ, foldAngles, folds, mode, center);
    addFoldLineSegment(arr, foldX, backZ, foldAngles, folds, mode, center);
  }}

  return new Float32Array(arr);
}}

function addFoldLineSegment(arr, foldX, z, foldAngles, folds, mode, center) {{
  const p1 = transformPoint({{ x: foldX, y: -config.height * 0.5 }}, foldAngles, folds, mode, z);
  const p2 = transformPoint({{ x: foldX, y: config.height * 0.5 }}, foldAngles, folds, mode, z);
  arr.push(p1.x - center.x, p1.y - center.y, p1.z - center.z);
  arr.push(p2.x - center.x, p2.y - center.y, p2.z - center.z);
}}

function drawMesh(texture, back) {{
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
}}

function drawFoldLines(matrix) {{
  if (!showFoldLinesCheckbox.checked) return;

  const positions = buildFoldLinePositions();
  if (!positions.length) return;

  gl.useProgram(lineProgram);
  gl.bindBuffer(gl.ARRAY_BUFFER, foldLineBuffer);
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
}}

function draw() {{
  resize();
  foldValue.textContent = foldSlider.value + '° / ' + foldSlider.max + '°';
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
  drawFoldLines(matrix);
}}

function resize() {{
  const dpr = Math.min(window.devicePixelRatio || 1, 2);
  const w = Math.floor(canvas.clientWidth * dpr);
  const h = Math.floor(canvas.clientHeight * dpr);
  if (canvas.width !== w || canvas.height !== h) {{
    canvas.width = w;
    canvas.height = h;
  }}
}}

function perspective(fov, aspect, near, far) {{
  const f = 1 / Math.tan(fov / 2);
  const nf = 1 / (near - far);
  return new Float32Array([
    f / aspect, 0, 0, 0,
    0, f, 0, 0,
    0, 0, (far + near) * nf, -1,
    0, 0, (2 * far * near) * nf, 0
  ]);
}}
function identity() {{
  return new Float32Array([1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1]);
}}
function multiply(a, b) {{
  const o = new Float32Array(16);
  for (let r = 0; r < 4; r++) {{
    for (let c = 0; c < 4; c++) {{
      o[c * 4 + r] = a[0 * 4 + r] * b[c * 4 + 0] + a[1 * 4 + r] * b[c * 4 + 1] + a[2 * 4 + r] * b[c * 4 + 2] + a[3 * 4 + r] * b[c * 4 + 3];
    }}
  }}
  return o;
}}
function translate(x, y, z) {{
  const m = identity();
  m[12] = x; m[13] = y; m[14] = z;
  return m;
}}
function rotateX(a) {{
  const s = Math.sin(a), c = Math.cos(a);
  return new Float32Array([1,0,0,0, 0,c,s,0, 0,-s,c,0, 0,0,0,1]);
}}
function rotateMatrixY(a) {{
  const s = Math.sin(a), c = Math.cos(a);
  return new Float32Array([c,0,-s,0, 0,1,0,0, s,0,c,0, 0,0,0,1]);
}}
function smoothstep(t) {{
  t = clamp(t, 0, 1);
  return t * t * (3 - 2 * t);
}}
function clamp(v, min, max) {{ return Math.max(min, Math.min(max, v)); }}
function round3(v) {{ return Math.round(v * 1000) / 1000; }}

canvas.addEventListener('pointerdown', e => {{
  state.dragging = true;
  state.lastX = e.clientX;
  state.lastY = e.clientY;
  canvas.setPointerCapture(e.pointerId);
}});
canvas.addEventListener('pointermove', e => {{
  if (!state.dragging) return;
  const dx = e.clientX - state.lastX;
  const dy = e.clientY - state.lastY;
  state.lastX = e.clientX;
  state.lastY = e.clientY;
  state.rotY += dx * 0.01;
  state.rotX += dy * 0.01;
  state.rotX = clamp(state.rotX, -1.45, 1.45);
  requestAnimationFrame(draw);
}});
canvas.addEventListener('pointerup', e => {{
  state.dragging = false;
  canvas.releasePointerCapture(e.pointerId);
}});
canvas.addEventListener('wheel', e => {{
  e.preventDefault();
  state.zoom *= Math.exp(-e.deltaY * 0.001);
  state.zoom = clamp(state.zoom, 0.35, 4);
  requestAnimationFrame(draw);
}}, {{ passive: false }});
canvas.addEventListener('dblclick', () => {{
  state.rotX = -0.58;
  state.rotY = 0.62;
  state.zoom = 1.15;
  requestAnimationFrame(draw);
}});
foldSlider.addEventListener('input', () => requestAnimationFrame(draw));
modeSelect.addEventListener('change', () => {{
  updateSliderLimit();
  requestAnimationFrame(draw);
}});
showFoldLinesCheckbox.addEventListener('change', () => requestAnimationFrame(draw));
window.addEventListener('resize', () => requestAnimationFrame(draw));
requestAnimationFrame(draw);
</script>
</body>
</html>";
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
