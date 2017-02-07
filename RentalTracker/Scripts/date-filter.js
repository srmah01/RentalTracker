$(function () {
    $('.hidden').hide().removeClass('hidden');

    $('.datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'DD/MM/YYYY'
    });

    $('#DateFilterSelection').change(function () {
        var str = "";
        $("select option:selected").each(function () {
            str = $(this).text();
        });
        
        if (str == "Custom Date") {
            $('#CustomDateDates').show();
        }
        else {
            $('#CustomDateDates').hide();
        }
    });
});