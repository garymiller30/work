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
.panel{position:fixed;left:14px;top:14px;width:min(760px,calc(100vw - 28px));max-height:calc(100vh - 72px);overflow:auto;background:rgba(20,22,26,.86);border:1px solid rgba(255,255,255,.12);border-radius:8px;padding:10px 12px;backdrop-filter:blur(8px);box-shadow:0 12px 30px rgba(0,0,0,.25)}
.toolbar{display:flex;gap:12px;align-items:center;justify-content:space-between;margin-bottom:8px;flex-wrap:wrap}
.toolbar label{display:flex;gap:8px;align-items:center;font-size:13px;white-space:nowrap}
.toolbar input[type=range]{width:110px}
.folds{display:grid;grid-template-columns:auto auto auto minmax(180px,1fr) auto;gap:7px 8px;align-items:center;font-size:12px}
.folds .head{color:rgba(255,255,255,.62)}
.folds .name{font-variant-numeric:tabular-nums;color:#dce4ea}
.folds input,.folds select,.toolbar button{background:#2c3038;color:#fff;border:1px solid rgba(255,255,255,.18);border-radius:5px}
.folds input[type=number]{width:62px;padding:4px 6px}
.folds input[type=range]{width:100%}
.folds select{padding:4px 6px}
.toolbar button{padding:5px 10px;cursor:pointer}
.toolbar button:hover{background:#38404c}
.hint{position:fixed;left:16px;bottom:12px;color:rgba(255,255,255,.62);font-size:12px}
@media(max-width:720px){.folds{grid-template-columns:auto auto minmax(120px,1fr) auto}.folds .side-head,.folds .side-cell{display:none}.toolbar{align-items:flex-start;flex-direction:column}}
</style>
</head>
<body>
<canvas id=""stage""></canvas>
<div class=""panel"">
  <div class=""toolbar"">
    <label><input id=""showCreases"" type=""checkbox"" checked> показати біговки</label>
    <label>радіус <input id=""baseRadius"" type=""range"" min=""0.1"" max=""4"" value=""0.3"" step=""0.1""><span id=""baseRadiusValue"">0.3</span></label>
    <label>зазор <input id=""layerGap"" type=""range"" min=""0"" max=""0.5"" value=""0.08"" step=""0.01""><span id=""layerGapValue"">0.08</span></label>
    <button id=""resetFolds"" type=""button"">скинути</button>
  </div>
  <div id=""folds"" class=""folds""></div>
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
  arcRadius: 0.3,
  arcRadiusStep: 0.45,
  paperHalfThickness: 0.01,
  foldedLayerGap: 0.08
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
const foldsPanel = document.getElementById('folds');
const resetFoldsButton = document.getElementById('resetFolds');
const baseRadiusSlider = document.getElementById('baseRadius');
const baseRadiusValue = document.getElementById('baseRadiusValue');
const layerGapSlider = document.getElementById('layerGap');
const layerGapValue = document.getElementById('layerGapValue');
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
initFoldControls();
syncMaterialControls();

function buildSamples(axis) {
  const size = axis === 'x' ? config.width : config.height;
  const creases = axisMatchesDirection(axis) ? config.creases : [];
  const step = Math.max(2, size / 150);
  const set = new Set();
  for (let v = 0; v <= size + 0.01; v += step) set.add(round3(Math.min(size, v)));
  set.add(0); set.add(round3(size));
  for (let creaseIndex = 0; creaseIndex < creases.length; creaseIndex++) {
    const f = creases[creaseIndex];
    const maxArc = Math.PI * (4 + foldSettings.arcRadiusStep * Math.max(0, config.creases.length - 1));
    for (let i = 0; i <= 28; i++) {
      set.add(round3(clamp(f - maxArc * 0.5 * i / 28, 0, size)));
      set.add(round3(clamp(f + maxArc * 0.5 * i / 28, 0, size)));
    }
  }
  return Array.from(set).sort((a,b) => a-b);
}

function initFoldControls() {
  foldsPanel.innerHTML = '';
  const headers = [
    ['head', 'Лінія'],
    ['head', 'Крок'],
    ['head side-head', 'Сторона'],
    ['head', 'Кут'],
    ['head', '°']
  ];
  for (const [cls, text] of headers) {
    const el = document.createElement('div');
    el.className = cls;
    el.textContent = text;
    foldsPanel.appendChild(el);
  }

  config.creases.forEach((crease, index) => {
    const rowName = document.createElement('div');
    rowName.className = 'name';
    rowName.textContent = (index + 1) + ' (' + crease.toFixed(1) + ' мм)';
    foldsPanel.appendChild(rowName);

    const order = document.createElement('input');
    order.type = 'number';
    order.min = '0';
    order.max = String(config.creases.length);
    order.step = '1';
    order.value = String(index + 1);
    order.dataset.foldIndex = String(index);
    order.dataset.foldField = 'order';
    foldsPanel.appendChild(order);

    const sideWrap = document.createElement('div');
    sideWrap.className = 'side-cell';
    const side = document.createElement('select');
    side.dataset.foldIndex = String(index);
    side.dataset.foldField = 'side';
    const before = document.createElement('option');
    before.value = 'before';
    before.textContent = config.direction === 'horizontal' ? 'ліва' : 'нижня';
    const after = document.createElement('option');
    after.value = 'after';
    after.textContent = config.direction === 'horizontal' ? 'права' : 'верхня';
    side.appendChild(before);
    side.appendChild(after);
    side.value = 'after';
    sideWrap.appendChild(side);
    foldsPanel.appendChild(sideWrap);

    const angle = document.createElement('input');
    angle.type = 'range';
    angle.min = '-180';
    angle.max = '180';
    angle.step = '1';
    angle.value = '0';
    angle.dataset.foldIndex = String(index);
    angle.dataset.foldField = 'angle';
    foldsPanel.appendChild(angle);

    const angleValue = document.createElement('input');
    angleValue.type = 'number';
    angleValue.min = '-180';
    angleValue.max = '180';
    angleValue.step = '1';
    angleValue.value = '0';
    angleValue.dataset.foldIndex = String(index);
    angleValue.dataset.foldField = 'angleValue';
    foldsPanel.appendChild(angleValue);
  });

  foldsPanel.addEventListener('input', handleFoldControlChange);
  foldsPanel.addEventListener('change', handleFoldControlChange);
  resetFoldsButton.addEventListener('click', resetFoldControls);
}

function handleFoldControlChange(event) {
  const field = event.target.dataset.foldField;
  const index = event.target.dataset.foldIndex;
  if (index !== undefined && (field === 'angle' || field === 'angleValue')) {
    const value = clamp(Number(event.target.value) || 0, -180, 180);
    const range = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""angle""]`);
    const number = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""angleValue""]`);
    if (range) range.value = String(value);
    if (number) number.value = String(value);
  }
  requestAnimationFrame(draw);
}

function resetFoldControls() {
  config.creases.forEach((_, index) => {
    const order = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""order""]`);
    const side = foldsPanel.querySelector(`select[data-fold-index=""${index}""][data-fold-field=""side""]`);
    const range = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""angle""]`);
    const number = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""angleValue""]`);
    if (order) order.value = String(index + 1);
    if (side) side.value = 'after';
    if (range) range.value = '0';
    if (number) number.value = '0';
  });
  requestAnimationFrame(draw);
}

function syncMaterialControls() {
  foldSettings.arcRadius = Number(baseRadiusSlider.value) || foldSettings.arcRadius;
  foldSettings.foldedLayerGap = Number(layerGapSlider.value) || 0;
  baseRadiusValue.textContent = foldSettings.arcRadius.toFixed(1);
  layerGapValue.textContent = foldSettings.foldedLayerGap.toFixed(2);
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

function currentFoldSteps() {
  const steps = [];
  config.creases.forEach((crease, index) => {
    const orderInput = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""order""]`);
    const sideInput = foldsPanel.querySelector(`select[data-fold-index=""${index}""][data-fold-field=""side""]`);
    const angleInput = foldsPanel.querySelector(`input[data-fold-index=""${index}""][data-fold-field=""angle""]`);
    const order = Math.max(0, Math.round(Number(orderInput ? orderInput.value : index + 1) || 0));
    const degrees = clamp(Number(angleInput ? angleInput.value : 0) || 0, -180, 180);
    if (order === 0 || Math.abs(degrees) < 0.001) return;
    steps.push({
      index,
      order,
      crease,
      side: sideInput ? sideInput.value : 'after',
      angle: -degrees * Math.PI / 180
    });
  });
  steps.sort((a, b) => a.order - b.order || a.index - b.index);
  for (let i = 0; i < steps.length; i++) steps[i].radius = foldRadius(i);
  steps.axes = buildFoldAxes(steps);
  return steps;
}

function foldRadius(foldIndex) {
  return foldSettings.arcRadius + foldSettings.arcRadiusStep * foldIndex;
}

function buildFoldAxes(steps) {
  const axes = [];
  for (let i = 0; i < steps.length; i++) {
    const source = creaseAxisPoint(axisSampleValue(steps[i]));
    const axis = {
      point: creaseAxisPoint(steps[i].crease),
      line: creaseLineDirection(),
      tangent: creasePanelDirection(),
      normal: { x: 0, y: 0, z: 1 }
    };
    for (let prev = 0; prev < i; prev++) {
      const angle = creaseArcAngle(source, steps[prev]);
      if (Math.abs(angle) > 0.0001) {
        axis.point = applyCreaseArcFold(axis.point, source, steps[prev], axes[prev]);
        axis.line = rotateVectorAroundAxis(axis.line, axes[prev].line, angle);
        axis.tangent = rotateVectorAroundAxis(axis.tangent, axes[prev].line, angle);
        axis.normal = rotateVectorAroundAxis(axis.normal, axes[prev].line, angle);
      }
    }
    const offset = layerOffsetUntil(source, steps, i);
    axis.point.x += axis.normal.x * offset;
    axis.point.y += axis.normal.y * offset;
    axis.point.z += axis.normal.z * offset;
    axes.push(axis);
  }
  return axes;
}

function axisSampleValue(step) {
  const sideSign = step.side === 'before' ? -1 : 1;
  return step.crease + sideSign * Math.max(0.01, step.radius);
}

function transformPoint(source, foldSteps, sideZ) {
  let p = { x: source.x, y: source.y, z: sideZ };
  let normal = { x: 0, y: 0, z: 1 };

  for (let i = 0; i < foldSteps.length; i++) {
    const angle = creaseArcAngle(source, foldSteps[i]);
    if (Math.abs(angle) > 0.0001) {
      p = applyCreaseArcFold(p, source, foldSteps[i], foldSteps.axes[i]);
      normal = rotateVectorAroundAxis(normal, foldSteps.axes[i].line, angle);
    }
  }

  const offset = layerOffset(source, foldSteps);
  p.x += normal.x * offset;
  p.y += normal.y * offset;
  p.z += normal.z * offset;
  return p;
}

function applyCreaseArcFold(p, source, step, axis) {
  if (Math.abs(step.angle) < 0.0001) return p;
  return applyCylindricalFold(p, source, step, axis);
}

function creaseArcAngle(source, step) {
  const value = creaseCoordinate(source);
  const sideSign = step.side === 'before' ? -1 : 1;
  const local = sideSign * (value - step.crease);
  const arcLength = step.radius * Math.abs(step.angle);
  const leftBoundary = -arcLength * 0.5;
  const rightBoundary = arcLength * 0.5;

  if (local <= leftBoundary) return 0;

  if (local < rightBoundary) {
    const t = (local - leftBoundary) / Math.max(0.0001, arcLength);
    return step.angle * t;
  }

  return step.angle;
}

function applyCylindricalFold(p, source, step, axis) {
  const value = creaseCoordinate(source);
  const sideSign = step.side === 'before' ? -1 : 1;
  const local = sideSign * (value - step.crease);
  const absAngle = Math.abs(step.angle);
  const arcLength = step.radius * absAngle;
  const leftBoundary = -arcLength * 0.5;
  const rightBoundary = arcLength * 0.5;
  if (local <= leftBoundary) return p;

  const relative = subtractPoint(p, axis.point);
  const dz = dot(relative, axis.normal);
  const lineOffset = dot(relative, axis.line);
  const bendSign = Math.sign(step.angle || 1) * foldSettings.bendDirection;
  let u;
  let z;
  let theta;

  if (local < rightBoundary) {
    theta = (local - leftBoundary) / Math.max(0.0001, step.radius);
    const signedTheta = Math.sign(step.angle || 1) * theta;
    u = leftBoundary + step.radius * Math.sin(theta);
    z = bendSign * step.radius * (1 - Math.cos(theta));
    const normal = rotateVectorAroundAxis(axis.normal, axis.line, signedTheta);
    return addPoints(
      axis.point,
      scaleVector(axis.line, lineOffset),
      scaleVector(axis.tangent, sideSign * u),
      scaleVector(axis.normal, z),
      scaleVector(normal, dz)
    );
  } else {
    const uEnd = leftBoundary + step.radius * Math.sin(absAngle);
    const zEnd = bendSign * step.radius * (1 - Math.cos(absAngle));
    const currentBoundary = addPoints(
      axis.point,
      scaleVector(axis.line, lineOffset),
      scaleVector(axis.tangent, sideSign * rightBoundary)
    );
    const arcEnd = addPoints(
      axis.point,
      scaleVector(axis.line, lineOffset),
      scaleVector(axis.tangent, sideSign * uEnd),
      scaleVector(axis.normal, zEnd)
    );
    const packageVector = subtractPoint(p, currentBoundary);
    const foldedVector = rotateVectorAroundAxis(packageVector, axis.line, step.angle);
    return addPoints(
      arcEnd,
      foldedVector
    );
  }
}

function rotateByDirection(p, axis, angle) {
  const axisPoint = axis.point || axis;
  const axisLine = axis.line || creaseLineDirection();
  return rotatePointAroundAxis(p, axisPoint, axisLine, angle);
}

function rotateVectorByDirection(v, angle) {
  return rotateVectorAroundAxis(v, creaseLineDirection(), angle);
}

function isOnMovingSide(value, step) {
  const eps = 0.001;
  return step.side === 'before'
    ? value < step.crease - eps
    : value > step.crease + eps;
}

function layerOffset(source, foldSteps) {
  return layerOffsetUntil(source, foldSteps, foldSteps.length);
}

function layerOffsetUntil(source, foldSteps, count) {
  let offset = 0;
  const value = creaseCoordinate(source);
  for (let i = 0; i < count; i++) {
    if (isOnMovingSide(value, foldSteps[i])) {
      offset += foldSettings.foldedLayerGap * (i + 1) * clamp(Math.abs(foldSteps[i].angle) / Math.PI, 0, 1);
    }
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

function creasePanelDirection() {
  return config.direction === 'horizontal'
    ? { x: 1, y: 0, z: 0 }
    : { x: 0, y: 1, z: 0 };
}

function creaseLineDirection() {
  return config.direction === 'horizontal'
    ? { x: 0, y: 1, z: 0 }
    : { x: 1, y: 0, z: 0 };
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

function rotateVectorY(v, angle) {
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  return {
    x: v.x * c + v.z * s,
    y: v.y,
    z: -v.x * s + v.z * c
  };
}

function rotateVectorX(v, angle) {
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  return {
    x: v.x,
    y: v.y * c - v.z * s,
    z: v.y * s + v.z * c
  };
}

function rotatePointAroundAxis(p, axisPoint, axisDirection, angle) {
  const relative = subtractPoint(p, axisPoint);
  const rotated = rotateVectorAroundAxis(relative, axisDirection, angle);
  return addPoints(axisPoint, rotated);
}

function rotateVectorAroundAxis(v, axisDirection, angle) {
  const axis = normalize(axisDirection);
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  const kDotV = dot(axis, v);
  const cross = {
    x: axis.y * v.z - axis.z * v.y,
    y: axis.z * v.x - axis.x * v.z,
    z: axis.x * v.y - axis.y * v.x
  };
  return {
    x: v.x * c + cross.x * s + axis.x * kDotV * (1 - c),
    y: v.y * c + cross.y * s + axis.y * kDotV * (1 - c),
    z: v.z * c + cross.z * s + axis.z * kDotV * (1 - c)
  };
}

function normalize(v) {
  const length = Math.hypot(v.x, v.y, v.z);
  if (length < 0.000001) return { x: 0, y: 0, z: 1 };
  return {
    x: v.x / length,
    y: v.y / length,
    z: v.z / length
  };
}

function subtractPoint(a, b) {
  return {
    x: a.x - b.x,
    y: a.y - b.y,
    z: a.z - b.z
  };
}

function scaleVector(v, scale) {
  return {
    x: v.x * scale,
    y: v.y * scale,
    z: v.z * scale
  };
}

function dot(a, b) {
  return a.x * b.x + a.y * b.y + a.z * b.z;
}

function addPoints() {
  const out = { x: 0, y: 0, z: 0 };
  for (let i = 0; i < arguments.length; i++) {
    out.x += arguments[i].x;
    out.y += arguments[i].y;
    out.z += arguments[i].z;
  }
  return out;
}

function buildPositions(back) {
  const foldSteps = currentFoldSteps();
  const sideZ = back ? -foldSettings.paperHalfThickness : foldSettings.paperHalfThickness;
  const raw = [];
  for (const y of ys) {
    const modelY = config.height * 0.5 - y;
    for (const x of xs) raw.push(transformPoint({ x, y: modelY }, foldSteps, sideZ));
  }
  const center = getModelCenter(foldSteps);
  const arr = [];
  for (const p of raw) arr.push(p.x - center.x, p.y - center.y, p.z - center.z);
  return new Float32Array(arr);
}

function getModelCenter(foldSteps) {
  const min = { x: Infinity, y: Infinity, z: Infinity };
  const max = { x: -Infinity, y: -Infinity, z: -Infinity };
  for (const y of ys) {
    const modelY = config.height * 0.5 - y;
    for (const x of xs) {
      const p = transformPoint({ x, y: modelY }, foldSteps, 0);
      min.x = Math.min(min.x, p.x); min.y = Math.min(min.y, p.y); min.z = Math.min(min.z, p.z);
      max.x = Math.max(max.x, p.x); max.y = Math.max(max.y, p.y); max.z = Math.max(max.z, p.z);
    }
  }
  return { x: (min.x + max.x) * 0.5, y: (min.y + max.y) * 0.5, z: (min.z + max.z) * 0.5 };
}

function buildCreaseLinePositions() {
  if (!showCreasesCheckbox.checked) return new Float32Array(0);

  const foldSteps = currentFoldSteps();
  const center = getModelCenter(foldSteps);
  const arr = [];
  const z = foldSettings.paperHalfThickness + 0.08;

  for (const crease of config.creases) {
    if (config.direction === 'horizontal') {
      addLinePoint(arr, { x: crease, y: -config.height * 0.5 }, foldSteps, z, center);
      addLinePoint(arr, { x: crease, y: config.height * 0.5 }, foldSteps, z, center);
    } else {
      addLinePoint(arr, { x: 0, y: crease - config.height * 0.5 }, foldSteps, z, center);
      addLinePoint(arr, { x: config.width, y: crease - config.height * 0.5 }, foldSteps, z, center);
    }
  }

  return new Float32Array(arr);
}

function addLinePoint(arr, point, foldSteps, z, center) {
  const p = transformPoint(point, foldSteps, z);
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
showCreasesCheckbox.addEventListener('change', () => requestAnimationFrame(draw));
baseRadiusSlider.addEventListener('input', () => {
  syncMaterialControls();
  requestAnimationFrame(draw);
});
layerGapSlider.addEventListener('input', () => {
  syncMaterialControls();
  requestAnimationFrame(draw);
});
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
