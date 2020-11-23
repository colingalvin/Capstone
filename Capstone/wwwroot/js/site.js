// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addTime(event) {
    $("#defaultTimes").append(
        '<input name="DefaultTimes" type="time" class="form-control" />'
    );
    event.preventDefault();
}