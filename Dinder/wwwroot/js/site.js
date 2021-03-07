// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AcceptFriendRequest(requesterID) {
    $.ajax({
        type: 'POST',
        url: '../../api/UserEntities/acceptFriendRequest/' + requesterID,
        contentType: 'application/x-www-form-urlencoded',
    });
}

function DeclineFriendRequest(requesterID) {
    $.ajax({
        type: 'POST',
        url: '../../api/UserEntities/declineFriendRequest/' + requesterID,
        contentType: 'application/x-www-form-urlencoded',
    });
}