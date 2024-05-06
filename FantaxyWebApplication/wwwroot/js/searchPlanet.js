$(document).ready(function () {

    try {
        checkModeratorAccess().then(function (hasAccess) {
            if (hasAccess) {
                console.log("Loading buttons")
                $('.frostPlanet').css('display', 'inline');
            } else {
                $('.frostPlanet').css('display', 'none');
            }
        }).catch(function (error) {
            console.error('Error checking moderator access: ' + error);
        });
    } catch (error) {
        console.error('Error checking moderator access: ' + error);
    }


    // Get the search input element
    // Set up click handlers for the links
    // (no need to set up click handlers if we're not using links)

    // Wait for the div-listItems element to be ready
    $('.div-listItems').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Main/PlanetPartial', function (data) {
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
        $.get('/Main/PlanetPartial?search=' + name, function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });

    $('.div-listItems').trigger('load')
});

