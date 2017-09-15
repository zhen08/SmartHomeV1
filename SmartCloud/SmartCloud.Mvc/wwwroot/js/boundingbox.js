function drawBoundingBox(canvasName, data) {
    var canvas = document.getElementById(canvasName);
    var ctx = canvas.getContext('2d');

    data.forEach(function (box) {
        ctx.strokeStyle = "red";
        ctx.beginPath();
        ctx.moveTo(box.x1 * 320 / 1024, box.y1 * 240 / 768);
        ctx.lineTo(box.x2 * 320 / 1024, box.y1 * 240 / 768);
        ctx.lineTo(box.x2 * 320 / 1024, box.y2 * 240 / 768);
        ctx.lineTo(box.x1 * 320 / 1024, box.y2 * 240 / 768);
        ctx.lineTo(box.x1 * 320 / 1024, box.y1 * 240 / 768);
        ctx.stroke();
    });
}
