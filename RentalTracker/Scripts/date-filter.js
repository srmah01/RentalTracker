$(function () {
    // Remove the hidden class so thay jQuery hide() and show() work unhindered
    $('.hidden').hide().removeClass('hidden');

    $('.datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'DD/MM/YYYY'
    }).on('dp.change', function (e) {
        // If any date changes, then re-validate the To Date
        if ($('#ToDate').val() !== "") {
            var validator = $("#DateFilterForm").validate();
            validator.element("#ToDate");
        }
    });

    // Validation rules for To Date
    $.validator.addMethod("dategreaterthan", function (value, element, param) {
        return Date.parse(value) >= Date.parse($('#' + param).val());
    }, "Date must not be earlier than other Date");

    $('#ToDate').rules("add", {
        dategreaterthan: { param: 'FromDate' }
    });

    // Handle changes to filter selection
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