import { checkModeratorAccess } from './moderator/getAccess.js';


$(document).ready(async function () {
    try {
        checkModeratorAccess().then(function (hasAccess) {
            if (hasAccess) {
                console.log("Loading buttons")
                $('.div-link').css('display', 'block');
            } else {
                $('.div-link').css('display', 'none');
            }
        }).catch(function (error) {
            console.error('Error checking moderator access: ' + error);
        });
    } catch (error) {
        console.error('Error checking moderator access: ' + error);
    }



    $('.div-listItems').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Main/UsersPartial', function (data) {
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
        $.get('/Main/UsersPartial?search=' + name, function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });     

    $('.div-listItems').trigger('load')
});