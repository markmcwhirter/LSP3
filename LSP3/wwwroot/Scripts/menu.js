jQuery(document).ready(function () {
    var Menu = [
        {
            text: "Home"
        },
        {
            text: "Sales",
            items: [
                { text: "Bulk Sales", url: "builder.html?page=BulkSales" }
            ]
        }
    ];

    $("#menuArea").append("<div class='nav' id='nav'></div>");
    $("#nav").html("<ul id='menu'><ul>");
    $("#menu").kendoMenu({
        openOnClick: true,
        dataSource: Menu,
        select: function (e) {
            if (e.item.outerText === "Home") {
                window.location = 'Default.html';
            } else if (e.item.outerText === "Bulk Sales") {
                window.location = 'builder.html?page=BulkSales';
            }
        }
    });

});