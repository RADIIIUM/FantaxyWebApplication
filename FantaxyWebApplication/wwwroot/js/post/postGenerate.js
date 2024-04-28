$(document).ready(function () {
    // Get the search input element
    // Set up click handlers for the links
    // (no need to set up click handlers if we're not using links)

    // Wait for the div-listItems element to be ready
    $('.postList').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Planet/PostList', function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.postList').html(data);
        });
    });

    // Trigger the load event on the div-listItems element
    $('.postList').trigger('load')
});