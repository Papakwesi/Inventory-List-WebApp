$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "timeOut": "3000"
    };

    if (toastrMessages.success && toastrMessages.success !== "") {
        toastr.success(toastrMessages.success);
    }
    if (toastrMessages.error && toastrMessages.error !== "") {
        toastr.error(toastrMessages.error);
    }
});
