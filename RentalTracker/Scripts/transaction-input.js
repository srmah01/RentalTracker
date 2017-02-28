$(function () {
    $('#transaction-datepicker').datetimepicker({
        showClear: true,
        showClose: true,
        format: 'DD/MM/YYYY'
    });

    $('#PayeeId').on('change', function() {
        var payeeValue = $(this).val();

        // Use a hidden select control that maps PayeeId -> DefaultCategoryId
        $('#PayeeCategoryMap').val(payeeValue);
        var categoryValue = $('#PayeeCategoryMap option:selected').text();
        $('#CategoryId').val(categoryValue);
    });
});

