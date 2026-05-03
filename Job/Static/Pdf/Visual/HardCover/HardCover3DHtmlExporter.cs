using JobSpace.Static.Pdf.Common;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;

namespace JobSpace.Static.Pdf.Visual.HardCover
{
    public static class HardCover3DHtmlExporter
    {
        private const int TextureDpi = 110;

        public static string ExportAndOpen(string pdfPath, HardCoverParams coverParams)
        {
            string htmlPath = Export(pdfPath, coverParams);

            Process.Start(new ProcessStartInfo
            {
                FileName = htmlPath,
                UseShellExecute = true
            });

            return htmlPath;
        }

        public static string Export(string pdfPath, HardCoverParams coverParams)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
                throw new ArgumentNullException(nameof(pdfPath));
            if (coverParams == null)
                throw new ArgumentNullException(nameof(coverParams));
            if (coverParams.Width <= 0 || coverParams.Height <= 0 || coverParams.Root <= 0)
                throw new ArgumentException("Width, height and root must be greater than zero.", nameof(coverParams));
            if (coverParams.Zagyn < 0 || coverParams.Rastav < 0)
                throw new ArgumentException("Zagyn and rastav cannot be negative.", nameof(coverParams));

            string front = RenderPageDataUri(pdfPath, 0, null);
            string back = RenderPageDataUri(pdfPath, 1, CreatePaperBackDataUri());

            string html = BuildHtml(
                Path.GetFileName(pdfPath),
                coverParams.Width,
                coverParams.Height,
                coverParams.Zagyn,
                coverParams.Rastav,
                coverParams.Root,
                coverParams.TotalWidth,
                coverParams.TotalHeight,
                front,
                back);

            string directory = Path.GetDirectoryName(pdfPath);
            string baseName = Path.GetFileNameWithoutExtension(pdfPath);
            string htmlPath = Path.Combine(directory, $"{baseName}_3d_hard_cover.html");

            File.WriteAllText(htmlPath, html, new UTF8Encoding(false));
            return htmlPath;
        }

        private static string RenderPageDataUri(string pdfPath, int pageIndex, string fallback)
        {
            int pageCount = PdfHelper.GetPageCount(pdfPath);
            if (pageIndex >= pageCount)
                return fallback ?? CreatePaperBackDataUri();

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

        private static string CreatePaperBackDataUri()
        {
            using (Bitmap bitmap = new Bitmap(24, 24))
            using (Graphics g = Graphics.FromImage(bitmap))
            using (MemoryStream stream = new MemoryStream())
            {
                g.Clear(Color.FromArgb(236, 229, 214));
                bitmap.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + System.Convert.ToBase64String(stream.ToArray());
            }
        }

        private static string BuildHtml(
            string title,
            double boardWidth,
            double boardHeight,
            double zagyn,
            double rastav,
            double root,
            double totalWidth,
            double totalHeight,
            string frontDataUri,
            string backDataUri)
        {
            return $@"<!doctype html>
<html lang=""uk"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width,initial-scale=1"">
<title>3D Hard Cover - {EscapeHtml(title)}</title>
<style>
html,body{{margin:0;height:100%;overflow:hidden;background:#202329;color:#f1f3f5;font-family:Segoe UI,Arial,sans-serif}}
#stage{{position:fixed;inset:0;width:100%;height:100%;display:block}}
.panel{{position:fixed;left:14px;top:14px;display:flex;gap:12px;align-items:center;background:rgba(20,22,26,.82);border:1px solid rgba(255,255,255,.12);border-radius:8px;padding:10px 12px;backdrop-filter:blur(8px);box-shadow:0 12px 30px rgba(0,0,0,.25)}}
.panel label{{display:flex;gap:8px;align-items:center;font-size:13px;white-space:nowrap}}
.panel input[type=range]{{width:240px}}
.panel input[type=checkbox]{{margin:0}}
.value{{min-width:44px;text-align:right;font-variant-numeric:tabular-nums}}
.hint{{position:fixed;left:16px;bottom:12px;color:rgba(255,255,255,.62);font-size:12px}}
@media(max-width:720px){{.panel{{right:14px;align-items:flex-start;flex-direction:column}}.panel input[type=range]{{width:calc(100vw - 140px)}}}}
</style>
</head>
<body>
<canvas id=""stage""></canvas>
<div class=""panel"">
  <label>Згин <input id=""fold"" type=""range"" min=""0"" max=""810"" value=""0"" step=""1""><span id=""foldValue"" class=""value"">0°</span></label>
  <label>X <input id=""panX"" type=""range"" min=""-200"" max=""200"" value=""0"" step=""1""><span id=""panXValue"" class=""value"">0 мм</span></label>
  <label><input id=""showLines"" type=""checkbox"" checked> лінії картонок</label>
  <label><input id=""showBack"" type=""checkbox"" checked> зворот</label>
</div>
<div class=""hint"">Миша: обертати. Колесо: масштаб. Подвійний клік: скинути вигляд.</div>
<script>
'use strict';
const config = {{
  title: {JsString(title)},
  boardWidth: {ToJsNumber(boardWidth)},
  boardHeight: {ToJsNumber(boardHeight)},
  zagyn: {ToJsNumber(zagyn)},
  rastav: {ToJsNumber(rastav)},
  root: {ToJsNumber(root)},
  totalWidth: {ToJsNumber(totalWidth)},
  totalHeight: {ToJsNumber(totalHeight)},
  front: {JsString(frontDataUri)},
  back: {JsString(backDataUri)}
}};

const hardCoverSettings = {{
  boardThickness: 3,
  boardWrapGap: 0.2,
  coverTopOffset: 0.28,
  coverBottomOffset: -0.08,
  cartonColor: [0.58, 0.48, 0.35, 1.0],
  cartonSideColor: [0.42, 0.34, 0.23, 1.0],
  lineColor: [0.05, 0.95, 0.45, 1.0]
}};

const canvas = document.getElementById('stage');
const gl = canvas.getContext('webgl', {{ antialias: true, alpha: false }});
if (!gl) throw new Error('WebGL is not supported.');

const textureVs = `
attribute vec3 aPosition;
attribute vec2 aUv;
uniform mat4 uMatrix;
varying vec2 vUv;
varying float vShade;
void main() {{
  vec4 p = uMatrix * vec4(aPosition, 1.0);
  gl_Position = p;
  vUv = aUv;
  vShade = clamp((aPosition.z + 120.0) / 240.0, 0.0, 1.0);
}}`;
const textureFs = `
precision mediump float;
uniform sampler2D uTexture;
uniform float uBack;
varying vec2 vUv;
varying float vShade;
void main() {{
  vec4 c = texture2D(uTexture, vUv);
  float shade = mix(0.88, 1.06, vShade);
  if (uBack > 0.5) shade *= 0.92;
  gl_FragColor = vec4(c.rgb * shade, 1.0);
}}`;
const solidVs = `
attribute vec3 aPosition;
attribute vec4 aColor;
uniform mat4 uMatrix;
varying vec4 vColor;
void main() {{
  gl_Position = uMatrix * vec4(aPosition, 1.0);
  vColor = aColor;
}}`;
const solidFs = `
precision mediump float;
varying vec4 vColor;
void main() {{
  gl_FragColor = vColor;
}}`;

function compile(type, source) {{
  const shader = gl.createShader(type);
  gl.shaderSource(shader, source);
  gl.compileShader(shader);
  if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) throw new Error(gl.getShaderInfoLog(shader));
  return shader;
}}
function link(vs, fs) {{
  const program = gl.createProgram();
  gl.attachShader(program, compile(gl.VERTEX_SHADER, vs));
  gl.attachShader(program, compile(gl.FRAGMENT_SHADER, fs));
  gl.linkProgram(program);
  if (!gl.getProgramParameter(program, gl.LINK_STATUS)) throw new Error(gl.getProgramInfoLog(program));
  return program;
}}

const textureProgram = link(textureVs, textureFs);
const textureLoc = {{
  position: gl.getAttribLocation(textureProgram, 'aPosition'),
  uv: gl.getAttribLocation(textureProgram, 'aUv'),
  matrix: gl.getUniformLocation(textureProgram, 'uMatrix'),
  texture: gl.getUniformLocation(textureProgram, 'uTexture'),
  back: gl.getUniformLocation(textureProgram, 'uBack')
}};

const solidProgram = link(solidVs, solidFs);
const solidLoc = {{
  position: gl.getAttribLocation(solidProgram, 'aPosition'),
  color: gl.getAttribLocation(solidProgram, 'aColor'),
  matrix: gl.getUniformLocation(solidProgram, 'uMatrix')
}};

const buffers = {{
  position: gl.createBuffer(),
  uv: gl.createBuffer(),
  index: gl.createBuffer(),
  solidPosition: gl.createBuffer(),
  solidColor: gl.createBuffer(),
  linePosition: gl.createBuffer(),
  lineColor: gl.createBuffer()
}};

const foldSlider = document.getElementById('fold');
const foldValue = document.getElementById('foldValue');
const panXSlider = document.getElementById('panX');
const panXValue = document.getElementById('panXValue');
const showLinesCheckbox = document.getElementById('showLines');
const showBackCheckbox = document.getElementById('showBack');

const layout = buildLayout();
const xs = buildSamples(config.totalWidth, [
  0,
  config.zagyn,
  layout.leftBoardRight,
  layout.leftTurnInSecond,
  layout.leftAxis,
  layout.rightAxis,
  layout.rightTurnInSecond,
  layout.rightBoardLeft,
  config.totalWidth - config.zagyn,
  config.totalWidth
]);
const ys = buildSamples(config.totalHeight, [
  0,
  layout.bottomTurnInSecond,
  config.zagyn,
  config.zagyn + config.boardHeight,
  layout.topTurnInSecond,
  config.totalHeight
]);
const uvsFront = buildUvs(false);
const uvsBack = buildUvs(true);

const state = {{
  rotX: -0.58,
  rotY: 0.68,
  zoom: 1.12,
  panX: 0,
  dragging: false,
  lastX: 0,
  lastY: 0
}};

function buildLayout() {{
  const leftBoardLeft = config.zagyn;
  const leftBoardRight = leftBoardLeft + config.boardWidth;
  const leftAxis = leftBoardRight + config.rastav;
  const rightAxis = leftAxis + config.root;
  const rightBoardLeft = rightAxis + config.rastav;
  const rightBoardRight = rightBoardLeft + config.boardWidth;
  const boardTop = config.zagyn + config.boardHeight;
  const wrapDepth = hardCoverSettings.boardThickness + hardCoverSettings.boardWrapGap;
  return {{
    leftBoardLeft, leftBoardRight, leftAxis, rightAxis, rightBoardLeft, rightBoardRight,
    boardBottom: config.zagyn,
    boardTop,
    bottomTurnInSecond: config.zagyn - wrapDepth,
    topTurnInSecond: boardTop + wrapDepth,
    leftTurnInSecond: leftBoardLeft - wrapDepth,
    rightTurnInSecond: rightBoardRight + wrapDepth,
    wrapDepth,
    centerX: config.totalWidth * 0.5,
    centerY: config.totalHeight * 0.5
  }};
}}

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

function foldPhases() {{
  const value = Number(foldSlider.value);
  return {{
    value,
    topTurnInAngle: clamp(value, 0, 180) * Math.PI / 180,
    bottomTurnInAngle: clamp(value - 180, 0, 180) * Math.PI / 180,
    leftTurnInAngle: clamp(value - 360, 0, 180) * Math.PI / 180,
    rightTurnInAngle: clamp(value - 540, 0, 180) * Math.PI / 180,
    coverAngle: clamp(value - 720, 0, 90) * Math.PI / 180
  }};
}}

function foldPhaseLabel(phases) {{
  const value = phases.value;
  if (value < 180) return 'верх ' + Math.round(phases.topTurnInAngle * 180 / Math.PI) + '°';
  if (value < 360) return 'низ ' + Math.round(phases.bottomTurnInAngle * 180 / Math.PI) + '°';
  if (value < 540) return 'ліва ' + Math.round(phases.leftTurnInAngle * 180 / Math.PI) + '°';
  if (value < 720) return 'права ' + Math.round(phases.rightTurnInAngle * 180 / Math.PI) + '°';
  return 'обкладинка ' + Math.round(phases.coverAngle * 180 / Math.PI) + '°';
}}

function buildSamples(max, hardStops) {{
  const step = Math.max(2, max / 180);
  const set = new Set();
  for (let v = 0; v <= max + 0.01; v += step) set.add(round3(Math.min(max, v)));
  for (const stop of hardStops) {{
    for (let i = -4; i <= 4; i++) set.add(round3(clamp(stop + i * step * 0.35, 0, max)));
  }}
  return Array.from(set).sort((a,b) => a-b);
}}

function buildGridIndices(nx, ny) {{
  const out = [];
  for (let y = 0; y < ny - 1; y++) {{
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

function buildCoverIndices() {{
  const phases = foldPhases();
  const hideLeftCorners = phases.leftTurnInAngle > 0.0001;
  const hideRightCorners = phases.rightTurnInAngle > 0.0001;
  const nx = xs.length;
  const out = [];

  for (let iy = 0; iy < ys.length - 1; iy++) {{
    const y0 = ys[iy];
    const y1 = ys[iy + 1];
    for (let ix = 0; ix < xs.length - 1; ix++) {{
      const x0 = xs[ix];
      const x1 = xs[ix + 1];
      if (shouldSkipCoverCell(x0, x1, y0, y1, hideLeftCorners, hideRightCorners))
        continue;

      const a = iy * nx + ix;
      const b = a + 1;
      const c = a + nx;
      const d = c + 1;
      out.push(a, c, b, b, c, d);
    }}
  }}

  return new Uint16Array(out);
}}

function shouldSkipCoverCell(x0, x1, y0, y1, hideLeftCorners, hideRightCorners) {{
  const inTopOrBottomTurnIn = y1 <= layout.boardBottom || y0 >= layout.boardTop;
  if (!inTopOrBottomTurnIn)
    return false;

  if (hideLeftCorners && x1 <= layout.leftBoardLeft)
    return true;
  if (hideRightCorners && x0 >= layout.rightBoardRight)
    return true;

  return false;
}}

function buildUvs(back) {{
  const arr = [];
  for (const y of ys) {{
    const v = 1 - y / config.totalHeight;
    for (const x of xs) {{
      const u = x / config.totalWidth;
      arr.push(back ? 1 - u : u, v);
    }}
  }}
  return new Float32Array(arr);
}}

function sourceYToModel(y) {{
  return layout.centerY - y;
}}

function transformPoint(source, z) {{
  let p = {{ x: source.x, y: sourceYToModel(source.y), z }};
  const phases = foldPhases();
  p = applyTurnInFold(p, source, phases);

  const angle = phases.coverAngle;
  if (Math.abs(angle) < 0.0001)
    return centerPoint(p);

  p = applyCoverHingeFold(p, source, angle);

  return centerPoint(p);
}}

function applyCoverHingeFold(p, source, angle) {{
  const hingeWidth = Math.max(config.rastav * 0.5, 0.001);

  if (source.x < layout.leftAxis) {{
    const factor = source.x <= layout.leftAxis - hingeWidth
      ? 1
      : smoothstep((layout.leftAxis - source.x) / hingeWidth);
    return bendIntoSpineY(p, layout.leftAxis, -angle, factor, hingeWidth);
  }}

  if (source.x > layout.rightAxis) {{
    const factor = source.x >= layout.rightAxis + hingeWidth
      ? 1
      : smoothstep((source.x - layout.rightAxis) / hingeWidth);
    return bendIntoSpineY(p, layout.rightAxis, angle, factor, hingeWidth);
  }}

  return p;
}}

function bendIntoSpineY(p, axisX, angle, factor, hingeWidth) {{
  if (factor <= 0) return p;

  const bent = rotatePointY(p, {{ x: axisX, z: 0 }}, angle * factor);
  const inwardDepth = Math.sin(Math.PI * factor) * hingeWidth * 0.5 * Math.sin(Math.abs(angle));
  bent.z -= inwardDepth;
  return bent;
}}

function applyTurnInFold(p, source, phases) {{
  if (config.zagyn <= 0) return p;

  const outsideLeft = source.x < layout.leftBoardLeft;
  const outsideRight = source.x > layout.rightBoardRight;
  const outsideBottom = source.y < layout.boardBottom;
  const outsideTop = source.y > layout.boardTop;

  if (outsideTop)
    p = applyDoubleTurnInX(p, source.y, layout.boardTop, layout.topTurnInSecond, phases.topTurnInAngle, 1);
  if (outsideBottom)
    p = applyDoubleTurnInX(p, source.y, layout.boardBottom, layout.bottomTurnInSecond, phases.bottomTurnInAngle, -1);
  if (outsideLeft)
    p = applyDoubleTurnInY(p, source.x, layout.leftBoardLeft, layout.leftTurnInSecond, phases.leftTurnInAngle, -1);
  if (outsideRight)
    p = applyDoubleTurnInY(p, source.x, layout.rightBoardRight, layout.rightTurnInSecond, phases.rightTurnInAngle, 1);

  return p;
}}

function applyDoubleTurnInY(p, sourceX, edgeX, secondX, angle, direction) {{
  if (Math.abs(angle) < 0.0001) return p;

  const firstAngle = clamp(angle, 0, Math.PI * 0.5) * direction;
  const secondAngle = clamp(angle - Math.PI * 0.5, 0, Math.PI * 0.5) * direction;
  const edgeAxis = {{ x: edgeX, z: 0 }};
  p = rotatePointY(p, edgeAxis, firstAngle);

  if (Math.abs(secondAngle) < 0.0001)
    return p;

  const foldsLeft = secondX < edgeX;
  if ((foldsLeft && sourceX > secondX) || (!foldsLeft && sourceX < secondX))
    return p;

  const secondAxis = rotatePointY({{ x: secondX, y: p.y, z: 0 }}, edgeAxis, firstAngle);
  return rotatePointY(p, secondAxis, secondAngle);
}}

function applyDoubleTurnInX(p, sourceY, edgeSourceY, secondSourceY, angle, direction) {{
  if (Math.abs(angle) < 0.0001) return p;

  const firstAngle = clamp(angle, 0, Math.PI * 0.5) * direction;
  const secondAngle = clamp(angle - Math.PI * 0.5, 0, Math.PI * 0.5) * direction;
  const edgeAxis = {{ y: sourceYToModel(edgeSourceY), z: 0 }};
  p = rotatePointX(p, edgeAxis, firstAngle);

  if (Math.abs(secondAngle) < 0.0001)
    return p;

  const foldsBottom = secondSourceY < edgeSourceY;
  if ((foldsBottom && sourceY > secondSourceY) || (!foldsBottom && sourceY < secondSourceY))
    return p;

  const secondAxis = rotatePointX({{ x: p.x, y: sourceYToModel(secondSourceY), z: 0 }}, edgeAxis, firstAngle);
  return rotatePointX(p, secondAxis, secondAngle);
}}

function centerPoint(p) {{
  return {{ x: p.x - layout.centerX, y: p.y, z: p.z }};
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

function rotatePointX(p, axis, angle) {{
  const s = Math.sin(angle);
  const c = Math.cos(angle);
  const dy = p.y - axis.y;
  const dz = p.z - axis.z;
  return {{
    x: p.x,
    y: axis.y + dy * c - dz * s,
    z: axis.z + dy * s + dz * c
  }};
}}

function buildCoverPositions(z) {{
  const arr = [];
  for (const y of ys) {{
    for (const x of xs) {{
      const p = transformPoint({{ x, y }}, z);
      arr.push(p.x, p.y, p.z);
    }}
  }}
  return new Float32Array(arr);
}}

function buildBoards() {{
  const top = 0;
  const bottom = -hardCoverSettings.boardThickness;
  const boards = [
    [layout.leftBoardLeft, layout.leftBoardRight, layout.boardBottom, layout.boardTop],
    [layout.leftAxis, layout.rightAxis, layout.boardBottom, layout.boardTop],
    [layout.rightBoardLeft, layout.rightBoardRight, layout.boardBottom, layout.boardTop]
  ];
  const positions = [];
  const colors = [];
  for (const b of boards) addBox(positions, colors, b[0], b[1], b[2], b[3], top, bottom);
  return {{
    positions: new Float32Array(positions),
    colors: new Float32Array(colors)
  }};
}}

function addBox(positions, colors, x0, x1, y0, y1, zTop, zBottom) {{
  addFace(positions, colors, [
    [x0,y0,zTop], [x1,y0,zTop], [x0,y1,zTop],
    [x1,y0,zTop], [x1,y1,zTop], [x0,y1,zTop]
  ], hardCoverSettings.cartonColor);
  addFace(positions, colors, [
    [x0,y1,zBottom], [x1,y1,zBottom], [x0,y0,zBottom],
    [x1,y1,zBottom], [x1,y0,zBottom], [x0,y0,zBottom]
  ], hardCoverSettings.cartonSideColor);
  addFace(positions, colors, [
    [x0,y0,zBottom], [x1,y0,zBottom], [x0,y0,zTop],
    [x1,y0,zBottom], [x1,y0,zTop], [x0,y0,zTop]
  ], hardCoverSettings.cartonSideColor);
  addFace(positions, colors, [
    [x0,y1,zTop], [x1,y1,zTop], [x0,y1,zBottom],
    [x1,y1,zTop], [x1,y1,zBottom], [x0,y1,zBottom]
  ], hardCoverSettings.cartonSideColor);
  addFace(positions, colors, [
    [x0,y0,zBottom], [x0,y0,zTop], [x0,y1,zBottom],
    [x0,y0,zTop], [x0,y1,zTop], [x0,y1,zBottom]
  ], hardCoverSettings.cartonSideColor);
  addFace(positions, colors, [
    [x1,y0,zTop], [x1,y0,zBottom], [x1,y1,zTop],
    [x1,y0,zBottom], [x1,y1,zBottom], [x1,y1,zTop]
  ], hardCoverSettings.cartonSideColor);
}}

function addFace(positions, colors, points, color) {{
  for (const point of points) {{
    const p = transformPoint({{ x: point[0], y: point[1] }}, point[2]);
    positions.push(p.x, p.y, p.z);
    colors.push(color[0], color[1], color[2], color[3]);
  }}
}}

function buildLines() {{
  const positions = [];
  const colors = [];
  const z = hardCoverSettings.coverTopOffset + 0.12;
  addRectLine(positions, colors, 0, config.totalWidth, 0, config.totalHeight, z);
  addRectLine(positions, colors, layout.leftBoardLeft, layout.leftBoardRight, layout.boardBottom, layout.boardTop, z);
  addRectLine(positions, colors, layout.leftAxis, layout.rightAxis, layout.boardBottom, layout.boardTop, z);
  addRectLine(positions, colors, layout.rightBoardLeft, layout.rightBoardRight, layout.boardBottom, layout.boardTop, z);
  addLine(positions, colors, layout.leftBoardRight, layout.boardBottom, layout.leftAxis, layout.boardBottom, z);
  addLine(positions, colors, layout.leftBoardRight, layout.boardTop, layout.leftAxis, layout.boardTop, z);
  addLine(positions, colors, layout.rightAxis, layout.boardBottom, layout.rightBoardLeft, layout.boardBottom, z);
  addLine(positions, colors, layout.rightAxis, layout.boardTop, layout.rightBoardLeft, layout.boardTop, z);
  return {{
    positions: new Float32Array(positions),
    colors: new Float32Array(colors)
  }};
}}

function addRectLine(positions, colors, x0, x1, y0, y1, z) {{
  addLine(positions, colors, x0, y0, x1, y0, z);
  addLine(positions, colors, x1, y0, x1, y1, z);
  addLine(positions, colors, x1, y1, x0, y1, z);
  addLine(positions, colors, x0, y1, x0, y0, z);
}}

function addLine(positions, colors, x0, y0, x1, y1, z) {{
  const dx = x1 - x0;
  const dy = y1 - y0;
  const length = Math.sqrt(dx * dx + dy * dy);
  const steps = Math.max(1, Math.ceil(length / 2));

  for (let i = 0; i < steps; i++) {{
    const t0 = i / steps;
    const t1 = (i + 1) / steps;
    const sx0 = x0 + dx * t0;
    const sy0 = y0 + dy * t0;
    const sx1 = x0 + dx * t1;
    const sy1 = y0 + dy * t1;

    if (shouldSkipLineSegment(sx0, sy0, sx1, sy1))
      continue;

    const p0 = transformPoint({{ x: sx0, y: sy0 }}, z);
    const p1 = transformPoint({{ x: sx1, y: sy1 }}, z);
    positions.push(p0.x, p0.y, p0.z, p1.x, p1.y, p1.z);
    for (let c = 0; c < 2; c++) colors.push(
      hardCoverSettings.lineColor[0],
      hardCoverSettings.lineColor[1],
      hardCoverSettings.lineColor[2],
      hardCoverSettings.lineColor[3]);
  }}
}}

function shouldSkipLineSegment(x0, y0, x1, y1) {{
  const phases = foldPhases();
  const mx = (x0 + x1) * 0.5;
  const my = (y0 + y1) * 0.5;
  return shouldSkipCoverCell(
    mx,
    mx,
    my,
    my,
    phases.leftTurnInAngle > 0.0001,
    phases.rightTurnInAngle > 0.0001);
}}

function drawCover(texture, uvData, z, back) {{
  gl.useProgram(textureProgram);
  gl.bindBuffer(gl.ARRAY_BUFFER, buffers.position);
  gl.bufferData(gl.ARRAY_BUFFER, buildCoverPositions(z), gl.DYNAMIC_DRAW);
  gl.enableVertexAttribArray(textureLoc.position);
  gl.vertexAttribPointer(textureLoc.position, 3, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ARRAY_BUFFER, buffers.uv);
  gl.bufferData(gl.ARRAY_BUFFER, uvData, gl.STATIC_DRAW);
  gl.enableVertexAttribArray(textureLoc.uv);
  gl.vertexAttribPointer(textureLoc.uv, 2, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, buffers.index);
  const coverIndices = buildCoverIndices();
  gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, coverIndices, gl.DYNAMIC_DRAW);

  gl.activeTexture(gl.TEXTURE0);
  gl.bindTexture(gl.TEXTURE_2D, texture);
  gl.uniform1i(textureLoc.texture, 0);
  gl.uniform1f(textureLoc.back, back ? 1 : 0);
  gl.enable(gl.CULL_FACE);
  gl.frontFace(gl.CCW);
  gl.cullFace(back ? gl.FRONT : gl.BACK);
  gl.drawElements(gl.TRIANGLES, coverIndices.length, gl.UNSIGNED_SHORT, 0);
  gl.disable(gl.CULL_FACE);
}}

function drawSolid(mesh, mode) {{
  gl.useProgram(solidProgram);
  gl.bindBuffer(gl.ARRAY_BUFFER, buffers.solidPosition);
  gl.bufferData(gl.ARRAY_BUFFER, mesh.positions, gl.DYNAMIC_DRAW);
  gl.enableVertexAttribArray(solidLoc.position);
  gl.vertexAttribPointer(solidLoc.position, 3, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ARRAY_BUFFER, buffers.solidColor);
  gl.bufferData(gl.ARRAY_BUFFER, mesh.colors, gl.DYNAMIC_DRAW);
  gl.enableVertexAttribArray(solidLoc.color);
  gl.vertexAttribPointer(solidLoc.color, 4, gl.FLOAT, false, 0, 0);

  gl.disable(gl.CULL_FACE);
  if (mode === gl.LINES) {{
    gl.depthMask(false);
    gl.lineWidth(2);
  }}
  gl.drawArrays(mode, 0, mesh.positions.length / 3);
  gl.depthMask(true);
}}

function draw() {{
  resize();
  const phases = foldPhases();
  state.panX = Number(panXSlider.value);
  foldValue.textContent = foldPhaseLabel(phases);
  panXValue.textContent = state.panX + ' мм';
  gl.viewport(0, 0, canvas.width, canvas.height);
  gl.clearColor(0.125, 0.137, 0.16, 1);
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
  gl.enable(gl.DEPTH_TEST);

  const aspect = canvas.width / Math.max(1, canvas.height);
  const maxDim = Math.max(config.totalWidth, config.totalHeight);
  const projection = perspective(42 * Math.PI / 180, aspect, 1, maxDim * 10);
  let view = identity();
  view = multiply(view, translate(0, 0, -maxDim * 2.2 / state.zoom));
  view = multiply(view, translate(state.panX, 0, 0));
  view = multiply(view, rotateX(state.rotX));
  view = multiply(view, rotateMatrixY(state.rotY));
  const matrix = multiply(projection, view);

  gl.useProgram(textureProgram);
  gl.uniformMatrix4fv(textureLoc.matrix, false, matrix);
  gl.useProgram(solidProgram);
  gl.uniformMatrix4fv(solidLoc.matrix, false, matrix);

  drawSolid(buildBoards(), gl.TRIANGLES);
  if (showBackCheckbox.checked) drawCover(textures.back, uvsBack, hardCoverSettings.coverBottomOffset, true);
  drawCover(textures.front, uvsFront, hardCoverSettings.coverTopOffset, false);
  if (showLinesCheckbox.checked) drawSolid(buildLines(), gl.LINES);
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
  state.rotY = 0.68;
  state.zoom = 1.12;
  panXSlider.value = '0';
  requestAnimationFrame(draw);
}});
foldSlider.addEventListener('input', () => requestAnimationFrame(draw));
panXSlider.addEventListener('input', () => requestAnimationFrame(draw));
showLinesCheckbox.addEventListener('change', () => requestAnimationFrame(draw));
showBackCheckbox.addEventListener('change', () => requestAnimationFrame(draw));
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
