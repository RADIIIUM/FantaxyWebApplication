$(document).ready(async function () {
    $('.div-listItems').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Chat/UserList', function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });


    $('.div-listItems').trigger('load')
});