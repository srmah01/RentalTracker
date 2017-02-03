$(function () {
    var tableInstance = $('table[id^="items-table"]');
    
    var trInstance = tableInstance.find('tr.clickable-row');
    trInstance.click(function () {
        var instance = $(this);
        var tableInstance = instance.closest('table');

        if (instance.hasClass('row-item-body')) {
            // remove all selected on table
            tableInstance.find('.row-item-body').removeClass('selected');
        }
        else {
            // And on any children
            tableInstance.find('.card-item-body').removeClass('selected');
        }
        var name = tableInstance.attr('id').split('-').pop();

        if (instance.hasClass('row-item-body')) {
            // Operate table rows
            instance.addClass('selected');
        }
        else {
            // operate on cards
            instance.find('.card-item-body').addClass('selected');
        }
        // Store the hidden editUrl in the session to mark which row was last selected
        sessionStorage[name + 'Selected'] = findUrl(instance, '#urle');

        enableButtons(instance);

    });

    var name = tableInstance.attr('id').split('-').pop();
    if (sessionStorage[name + 'Selected'] != undefined) {
        // this table has had a row seleceted
        var rows = $('tr', '#items-table-' + name);
        rows.each(function (index, value) {
            var instance = $(value);
            var editUrl = findUrl(instance, '#urle');
            // match the urle and make the matching item selected
            if (editUrl === sessionStorage[name + 'Selected']) {
                if (instance.hasClass('row-item-body')) {
                    // Operate table rows
                    instance.addClass('selected');
                }
                else {
                    instance.find('.card-item-body').addClass('selected');
                }

                enableButtons(instance);
                return false;
            }
        });
    }

    function enableButtons(row) {
        $('#edit').removeClass('disabled').attr('href', findUrl(row, '#urle'));
        $('#details').removeClass('disabled').attr('href', findUrl(row, '#urld'));
        $('#delete').removeClass('disabled').attr('href', findUrl(row, '#urlr'));
    }

    function findUrl(row, name) {
        return row.find(name).attr('href');
    }
});

