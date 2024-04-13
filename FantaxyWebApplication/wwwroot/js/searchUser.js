$(document).ready(function () {
    // Get the search input element
    // Set up click handlers for the links
    // (no need to set up click handlers if we're not using links)

    // Wait for the div-listItems element to be ready
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