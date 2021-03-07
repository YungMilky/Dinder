// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AcceptFriendRequest(requesterID) {
    console.log(requesterID);

    var data = {
        requesterID: requesterID
    };

    $.ajax({
        type: 'POST',
        url: '../../api/UserEntities/acceptFriendRequest/' + requesterID,
        contentType: 'application/x-www-form-urlencoded',
    });
}

function DeclineFriendRequest(userid) {
    console.log(userid);

    var data = {
        Friend1ID: userid,
        Friend2ID: currentProfileID
    };
    $("#friend-status").text("Add me!");

    $.ajax({
        type: 'POST',
        url: '../../api/UserEntities/declineFriendRequest',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data)
    });
}