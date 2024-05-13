$(document).ready(async function () {
    $.ajax({
        type: 'GET',
        url: '/Moderator/GetPlanetAccess',
        contentType: 'application/json; charset=utf-8'
    }).then(function (response) {
        if (response.success) {
            console.log("Loading buttons")
            $('.div-link').css('display', 'block');
        } else {
            $('.div-link').css('display', 'none');
        }
    }).fail(function (xhr, status, error) {
        console.error('Error sending message: ' + error);
        throw error; // Rethrow the error
    });


    $('.div-listItems').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Planet/UserList', function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });

    // Trigger the load event on the div-listItems element
    $('#searchInput').click(function (e) {
        e.preventDefault();
        var name = $('#search').val();
        name = encodeURIComponent(name);
        console.log(name);
        $.get('/Planet/UserList?search=' + name, function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });

    $('.div-listItems').trigger('load')
});