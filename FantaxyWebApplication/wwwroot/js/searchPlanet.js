$(document).ready(function () {
    // Get the search input element
    var searchInput = $('#search');

    // Set up click handlers for the links
    // (no need to set up click handlers if we're not using links)

    // Wait for the div-listItems element to be ready
    $('.div-listItems').on('load', function () {
        // Get the search term
        var searchTerm = searchInput.val();

        // Load the PlanetPartial view with the search term
        $.get('/Main/PlanetPartial?search=' + encodeURIComponent(searchTerm), function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.div-listItems').html(data);
        });
    });

    // Trigger the load event on the div-listItems element
    $('.div-listItems').trigger('load');
});