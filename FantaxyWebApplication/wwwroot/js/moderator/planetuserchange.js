let roles = [];
let currentIndex = 0;
$(document).ready(function () {


    $.ajax({
        type: 'GET',
        url: '/Moderator/GetPlanetAccess',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            const role = $('#UserRole').val();
            if (response.success) {
                roles = ["Администратор", "Модератор", "Пользователь"];
                console.log('Loading moderator functions');
                if (response.role == 3 && role == 'Пользователь') {
                    if (role == 'Заблокирован') {
                        // Do something
                        $('.maincontent').append('<input type="button" onclick="Unban()" class= "btn -dark -big" style = "background: green;" value = "Unban" /> ');
                    }
                    else {
                        $('.maincontent').append('<input type="button" onclick="Ban()" class= "btn -dark -big" style = "background: red;" value = "Ban" /> ');
                    }
                }

                if (response.role == 2) {
                    roles = roles.slice(1);
                }
                if (response.role == 1) {
                    roles = ["Владелец","Администратор", "Модератор", "Пользователь"];
                }

                if ((response.role == 2 && (role != 'Администратор' && role != 'Владелец')) || response.role == 1) {
                    $('.modal-content').css('display', "block")
                    if (role == 'Заблокирован') {
                        // Do something
                        $('.maincontent').append('<input type="button" onclick="Unban()" class= "btn -dark -big" style = "background: green;" value = "Unban" /> ');
                    }
                    else {
                        $('.maincontent').append('<input type="button" onclick="Ban()" class= "btn -dark -big" style = "background: red;" value = "Ban" /> ');
                    }

                }
                else {
                    $('.modal-content').css('display', "none");
                }

            }
            else {
                console.log('Not Moderator access');
            }
        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request
            console.error('Error sending message: ' + error);
        }
    });
});

function applyChangeRole() {
    const login = $('#UserLogin').val();
    const role = $('#roleBtn').html();

    $.ajax({
        url: '/Moderator/ChangePlanetRole',
        type: 'POST',
        contentType: 'application/json', // Add this line
        data: JSON.stringify({ Login: login, Role: role }),
        success: function (response) {
            if (response.success) {
                alert(`Роль ${login} была изменена`);
                location.reload();
            }
            else {
                alert(`ERROR`);
            }

        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request
            console.error('Error sending message: ' + error);
        }
    });
}

function changeRole() {
    $('#roleBtn').html(roles[currentIndex]);
    currentIndex = (currentIndex + 1) % roles.length;
}

function Ban() {
    const login = $('#UserLogin').val();
    $.ajax({
        url: '/Moderator/PlanetUserBan',
        type: 'POST',
        contentType: 'application/json', // Add this line
        data: JSON.stringify({ Login: login }),
        success: function (response) {
            if (response.success) {
                alert(`${login} has been banned`);
                history.back();
            }
            else {
                alert(`ERROR`);
            }

        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request
            console.error('Error sending message: ' + error);
        }
    });
}





function Unban() {
    const login = $('#UserLogin').val();
    $.ajax({
        url: '/Moderator/PlanetUserUnban',
        type: 'POST',
        contentType: 'application/json', // Add this line
        data: JSON.stringify({ Login: login }),
        success: function (response) {
            alert(`${login} has been unbanned`);
            history.back();
        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request
            console.error('Error sending message: ' + error);
        }
    });
}

