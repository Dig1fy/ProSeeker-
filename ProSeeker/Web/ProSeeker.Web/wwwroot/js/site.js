// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getConfirmation() {
    var retVal = confirm("Сигурни ли сте, че искате да изтриете това съдържание?");
    return retVal === true;
}