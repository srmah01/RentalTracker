$(function () {
    $('.hidden').hide().removeClass('hidden');

    $('.datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'DD/MM/YYYY'
    });

    $('#DateFilter').change(function () {
        var str = $("select option:selected").text();

        if (str == "Custom Date") {
            $('#FromDate').val('');
            $('#ToDate').val('');
            $('#CustomDateDates').show();
        }
        else {
            $('#FromDate').val('');
            $('#ToDate').val('');
            $('#CustomDateDates').hide();
        }
    });

    // A little frig to ensure the value of the DateFilter is set 
    // to the last known value for the filter action.
    // When clicking on the Date Sort action link the page always seems to display
    // first option in the list even though the Model has the correct value.
    // Created a hidden field with the value as a convenience to easily set the selection here.
    var selectId = $('#DateFilterValue').attr('data-value-id');
    if (selectId != 'undefined') {
        $('#DateFilter').val(selectId);
    }
});