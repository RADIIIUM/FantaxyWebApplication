/* ���������� ������ ������� ������ � ������� 250 �������� � ��������� */
function openNav() {
    document.getElementById("mySidenav").style.width = "100px";
}

/* ���������� ������ ������� ��������� � 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

// ������������ �������� ����
document.onmousemove = function (e) {
    var x = e.pageX;
    var y = e.pageY;
    var sidenav = document.getElementById("mySidenav");
    if (x > 120 && sidenav.style.width == "100px") {
        closeNav();
    }
}

// ������������ ����� ��� ������� ������ ���������
window.onclick = function (event) {
    if (event.target == document.getElementById("mySidenav")) {
        closeNav();
    }
}