
window.clienttime =
{


    showTime: function startTime() {
        var today = new Date(),
            h = checkTime(today.getHours()),
            m = checkTime(today.getMinutes()),
            s = checkTime(today.getSeconds());
        document.getElementById('time-hour').innerHTML = h;
        document.getElementById('time-minute').innerHTML = m;
        document.getElementById('time-second').innerHTML = s;
        t = setTimeout(function () {
            startTime()
        }, 500);
    }

}


function checkTime(i) {
    return (i < 10) ? "0" + i : i;
};