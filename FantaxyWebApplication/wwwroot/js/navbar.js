/* ”становить ширину боковой панели с шириной 250 пикселей и следующий */
function openNav() {
    document.getElementById("mySidenav").style.width = "100px";
}

/* ”становите ширину боковой навигации в 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

// ќтслеживание движени€ мыши
document.onmousemove = function (e) {
    var x = e.pageX;
    var y = e.pageY;
    var sidenav = document.getElementById("mySidenav");
    if (x > 120 && sidenav.style.width == "100px") {
        closeNav();
    }
}

// ќтслеживание клика вне боковой панели навигации
window.onclick = function (event) {
    if (event.target == document.getElementById("mySidenav")) {
        closeNav();
    }
}