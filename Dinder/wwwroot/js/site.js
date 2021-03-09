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

$(document).ready(function () {
    //can't set in html or c# yells at you
    $("#search-input").attr("asp-for", "searchQuery");

    //keep search query between pages (same as tabs logic for Profile.cshtml)
    $("#search-input").on('change input', function () {
        localStorage.setItem('searchQuery', $("#search-input").val());
    });

    //keep search query between pages
    var searchQuery = localStorage.getItem('searchQuery');
    if (searchQuery) {
        $("#search-input").val(searchQuery);
    }

    //enable search button when text is entered
    $("#search-input").on("input", function () {
        if ($("#search-input").val() != "") {
            $("#search-button").css("background-color", "#e84a5f");
            $("#search-button").prop("disabled", false);
        } else {
            $("#search-button").css("background-color", "#2d2d2d");
            $("#search-button").prop("disabled", true);
        }
    });
    if ($("#search-input").val() != "") {
        $("#search-button").css("background-color", "#e84a5f");
        $("#search-button").prop("disabled", false);
    }
});