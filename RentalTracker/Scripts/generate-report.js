$(function () {
    var yearNow = new Date().getFullYear();

    $('#year-datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'YYYY',
        minDate: (yearNow - 7).toString(),
        maxDate: (yearNow + 1).toString()
    }).on('dp.show', function (e) {
        // Ensure the viewMode is years everytime the date picker is opened.
        $('#year-datepicker').data("DateTimePicker").viewMode('years');
    });
});