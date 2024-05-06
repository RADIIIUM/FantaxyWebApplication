
export function checkModeratorAccess() {
    return $.ajax({
        type: 'GET',
        url: '/Moderator/GetAccess',
        contentType: 'application/json; charset=utf-8'
    }).then(function (response) {
        if (response.success) {
            console.log('Loading moderator functions');
            return true; // Resolve with true
        } else {
            console.log('Not Moderator access');
            return false; // Resolve with false
        }
    }).fail(function (xhr, status, error) {
        console.error('Error sending message: ' + error);
        throw error; // Rethrow the error
    });
}