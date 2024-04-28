$(document).ready(function () {
    // Get the search input element
    // Set up click handlers for the links
    // (no need to set up click handlers if we're not using links)

    // Wait for the div-listItems element to be ready
    $('.chatList').on('load', function () {
        // Get the search term
        // Load the UsersPartial view with the search term
        $.get('/Chat/ChatPartial', function (data) {
            // Replace the contents of the div-listItems div with the partial view
            $('.chatList').html(data);
        });
    });

    // Trigger the load event on the div-listItems element
    $('.chatList').trigger('load')
});