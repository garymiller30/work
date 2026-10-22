using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;

namespace JobSpace.Static.Pdf.Visual.TableCalendar
{
    /// <summary>Exports the three parts of a table-calendar sheet as a folded CSS 3D model.</summary>
    public static class TableCalendar3DHtmlExporter
    {
        public static string ExportAndOpen(string pdfPath, Bitmap preview, double pageWidth, double pageHeight, double baseWidth)
        {
            string htmlPath = Export(pdfPath, preview, pageWidth, pageHeight, baseWidth);
            Process.Start(new ProcessStartInfo { FileName = htmlPath, UseShellExecute = true });
            return htmlPath;
        }

        public static string Export(string pdfPath, Bitmap preview, double pageWidth, double pageHeight, double baseWidth)
        {
            if (preview == null) throw new ArgumentNullException(nameof(preview));
            if (pageWidth <= 0 || pageHeight <= 0) throw new ArgumentException("Page dimensions must be greater than zero.");
            if (baseWidth <= 0 || baseWidth >= pageHeight) throw new ArgumentException("Base width must fit inside the page height.", nameof(baseWidth));

            string htmlPath = Path.Combine(Path.GetDirectoryName(pdfPath), Path.GetFileNameWithoutExtension(pdfPath) + "_3d_table_calendar.html");
            File.WriteAllText(htmlPath, BuildHtml(Path.GetFileName(pdfPath), ToDataUri(preview), pageWidth, pageHeight, baseWidth), new UTF8Encoding(false));
            return htmlPath;
        }

        private static string ToDataUri(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + System.Convert.ToBase64String(stream.ToArray());
            }
        }

        private static string BuildHtml(string title, string image, double width, double height, double baseWidth)
        {
            string html = @"<!doctype html>
<html lang=""uk""><head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1""><title>3D календар - __TITLE__</title>
<style>
html,body{margin:0;height:100%;overflow:hidden;background:linear-gradient(#f8f9fa,#e2e6ea);font-family:Segoe UI,Arial,sans-serif}
#stage{position:fixed;inset:0;perspective:1450px;perspective-origin:50% 42%;touch-action:none}
#model{
    position:absolute;
    transform-style:preserve-3d;
    transform-origin:50% 0;
    will-change:transform
}

.part {
  position: absolute;
  left: 0;
  top: 0;
  box-sizing: border-box;
  box-shadow: 0 14px 20px #0003;
  transform-style: preserve-3d; /* Чистий 3D-контейнер без конфліктних масок */
  background: transparent; 
}

/* Внутрішній блок для обрізки зображення */
.crop {
  position: absolute;
  inset: 0;
  overflow: hidden; /* Безпечно обрізає зайве, не ламаючи 3D */
  backface-visibility: hidden;
  -webkit-backface-visibility: hidden;
}

.part img {
  position: absolute;
  left: 0;
  width: 100%;
  height: auto;
  max-width: none;
  user-select: none;
  pointer-events: none;
  background:white;
}

.back {
  position: absolute;
  left: 0;
  top: 0;
  inset: 0;
  width: 100%;
  height: 100%;
  background: white;
  backface-visibility: hidden;
  -webkit-backface-visibility: hidden;
  transform: rotateY(180deg);
  z-index: -1;
}

#one{transform-origin:50% 0}
#base{transform-origin:50% 0}
#two{transform-origin:50% 0}
#shadow{position:absolute;border-radius:50%;background:#0004;filter:blur(15px);pointer-events:none}
.panel{position:fixed;left:14px;top:14px;padding:10px 12px;border-radius:8px;background:#ffffffdf;box-shadow:0 8px 26px #0003;font-size:13px;z-index:2}
.panel label{display:flex;gap:8px;align-items:center}
.panel input{width:180px}
.hint{position:fixed;bottom:12px;left:16px;color:#495057;font-size:12px;z-index:2}

</style></head><body>
<div id=""stage"">
    <div id=""shadow""></div>
    <div id=""model"">
        <div id=""one"" class=""part"">
            <div class=""crop""><img id=""oneImage"" alt=""Частина 1""/></div>
            <div class=""back""></div>
        </div>
        <div id=""base"" class=""part"">
            <div class=""crop""><img id=""baseImage"" alt=""Основа""></div>
            <div class=""back""></div>
        </div>
        <div id=""two"" class=""part"">
            <div class=""crop""><img id=""twoImage"" alt=""Частина 2""/></div>
            <div class=""back""></div>
        </div>
    </div>
</div>
<div class=""panel""><label>Поворот <input id=""yaw"" type=""range"" min=""-55"" max=""55"" value=""-24""><span id=""yawValue""></span></label></div><div class=""hint"">Частини 1 і 2 складені по краях основи. Перетягуйте мишею для огляду; колесо змінює масштаб.</div>
<script>'use strict';
const cfg={
    image:__IMAGE__,
    width:__WIDTH__,
    height:__HEIGHT__,
    base:__BASE__
};
const stage=document.getElementById('stage'),model=document.getElementById('model'),one=document.getElementById('one'),base=document.getElementById('base'),two=document.getElementById('two'),shadow=document.getElementById('shadow'),images=[document.getElementById('oneImage'),document.getElementById('baseImage'),document.getElementById('twoImage')],yaw=document.getElementById('yaw'),yawValue=document.getElementById('yawValue');
const state={yaw:-25,pitch:-12,zoom:1,drag:false,x:0,y:0};images.forEach(i=>i.src=cfg.image);
function crop(image,offset){image.style.top=(-offset)+'px'}
function layout(){
 const side=(cfg.height-cfg.base)/2;
 if (side<=0) return;
 const scale=Math.min((innerWidth*.69)/cfg.width,(innerHeight*.68)/side);
 const w=cfg.width*scale,sidePx=side*scale,basePx=cfg.base*scale;
 const fold=Math.asin(Math.min(.98,basePx/(2*sidePx)))*180/Math.PI,vertical=sidePx*Math.cos(fold*Math.PI/180);
 model.style.width=w+'px';
 model.style.height=sidePx+'px';
 model.style.left=(innerWidth-w)/2+'px';
 model.style.top=(innerHeight+vertical)/2+'px';
 one.style.width=two.style.width=base.style.width=w+'px';
 one.style.height=two.style.height=sidePx+'px';
 base.style.height=basePx+'px';
 one.style.top=(-sidePx)+'px';
 two.style.top='0px';
 base.style.top='0px';
 images.forEach(i=>{i.style.width=w+'px'});
 crop(images[0],0);
 crop(images[1],sidePx);
 crop(images[2],sidePx+basePx);
 // The centre strip is the bottom of the triangle.  Segment 1 is hinged to its
 // near edge and segment 2 to its far edge; their free edges meet at the apex.
 one.style.transformOrigin='50% 100%';
 two.style.transformOrigin='50% 0';
 base.style.transformOrigin='50% 0';
 one.style.transform='rotateX('+fold+'deg)';
 base.style.transform='rotateX(-90deg)';
 two.style.transform='translateZ('+(-basePx)+'px) rotateX('+(180-fold)+'deg)';
 shadow.style.width=(w*1.12)+'px';
 shadow.style.height=(sidePx*.34)+'px';
 shadow.style.left=(innerWidth-w*1.12)/2+'px';
 shadow.style.top=(innerHeight+vertical*.78)/2+'px';
 draw()}
function draw(){
    model.style.transform='rotateX('+state.pitch+'deg) rotateY('+state.yaw+'deg) scale('+state.zoom+')';
    yaw.value=state.yaw;
    yawValue.textContent=Math.round(state.yaw)+'°'
}
yaw.oninput=()=>{
    state.yaw=+yaw.value;
    draw()};
stage.onpointerdown=e=>{
    state.drag=true;
    state.x=e.clientX;
    state.y=e.clientY;
    stage.setPointerCapture(e.pointerId)};
    stage.onpointermove=e=>{
        if(!state.drag)return;
        state.yaw=Math.max(-180,Math.min(180,state.yaw+(e.clientX-state.x)*.35));
        state.pitch=Math.max(-30,Math.min(20,state.pitch+(e.clientY-state.y)*.12));
        state.x=e.clientX;
        state.y=e.clientY;draw()};
        stage.onpointerup=e=>{
            state.drag=false;
            stage.releasePointerCapture(e.pointerId)};
            stage.onwheel=e=>{e.preventDefault();
            state.zoom=Math.max(.55,Math.min(1.55,state.zoom*Math.exp(-e.deltaY*.001)));
            draw()
        };
    stage.ondblclick=()=>{
        state.yaw=-25;
        state.pitch=-12;
        state.zoom=1;
        draw()
    };
    addEventListener('resize',layout);
    images[0].onload=layout;
    layout();
</script></body></html>";
            return html.Replace("__TITLE__", EscapeHtml(title)).Replace("__IMAGE__", JsString(image)).Replace("__WIDTH__", Number(width)).Replace("__HEIGHT__", Number(height)).Replace("__BASE__", Number(baseWidth));
        }

        private static string Number(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);
        private static string JsString(string value) => "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n").Replace("<", "\\u003c") + "\"";
        private static string EscapeHtml(string value) => value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
    }
}
