$(document).ready(function () {
    $("body").on("contextmenu", function (e) {
        PreventDefaultEvent(e);
    });
});

function PreventDefaultEvent(e) {
    //swal('Right click disabled!');
    e.preventDefault();
    return false;
}