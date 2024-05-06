import { checkModeratorAccess } from './moderator/getAccess.js';

$(document).ready(function () {

    try {
        checkModeratorAccess().then(function (hasAccess) {
            if (hasAccess) {
                console.log("Loading buttons")
                $('#frostPlanet').css('display', 'block');
            } else {
                $('#frostPlanet').css('display', 'none');
            }
        }).catch(function (error) {
            console.error('Error checking moderator access: ' + error);
        });
    } catch (error) {
        console.error('Error checking moderator access: ' + error);
    }
});