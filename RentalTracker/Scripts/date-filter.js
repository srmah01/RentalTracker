$(function () {
    $('.hidden').hide().removeClass('hidden');

    $('.datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'DD/MM/YYYY'
    });

    $('#DateFilter').change(function () {
        var str = "";
        $("select option:selected").each(function () {
            str = $(this).text();
        });
        
        if (str == "Custom Date") {
            $('#CustomDateDates').show();
        }
        else {
            $('#FromDate').val('');
            $('#ToDate').val('');
            $('#CustomDateDates').hide();
        }
    });
});