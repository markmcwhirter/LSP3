
const columnDefs = [
    { field: 'authorID', headerName: 'AuthorID', sortable: true },
    { field: 'author', headerName: 'Author', sortable: true },
    { field: 'bookID', headerName: 'BookID', sortable: true, cellDataType: 'number', filter: 'agNumberColumnFilter' },
    { field: 'title', flex: 2, headerName: 'Title' },
    { field: 'subtitle', flex: 2, headerName: 'SubTitle' },
    { field: 'isbn', headerName: 'ISBN' },
    { field: 'notes', headerName: 'Notes' },
    { field: 'infoLink', headerName: 'Info', sortable: false, filter: false, editable: false, cellRenderer: params => { return "<a href='BookInfo?" + params.value + "'>Info</a>"; } },
    { field: 'editLink', headerName: 'Edit', sortable: false, filter: false, editable: false, cellRenderer: params => { return "<a href='Book?" + params.value + "'>Edit</a>"; } },
    { field: 'deleteLink', headerName: 'Delete', sortable: false, filter: false, editable: false, cellRenderer: params => { return "<a href='BookDelete?" + params.value + "'>Delete</a>"; } }
];

const gridOptions = {
    columnDefs: columnDefs,
    pagination: true,
    paginationPageSize: 20,
    rowHeight: 32.5,
    defaultColDef: {
        filter: "agTextColumnFilter",
        floatingFilter: true,
        editable: false,
        sortable: true,
        flex: 1
    },

    rowModelType: 'serverSide',


    serverSideDatasource: {
        getRows: (params) => {

            const { startRow, endRow, sortModel } = params.request;
            let sortColumn = "Author";
            let sortDirection = "ASC";
            let filter = JSON.stringify(params.request.filterModel);

            // Extract sorting information
            if (sortModel.length > 0) {
                sortColumn = sortModel[0].colId;
                sortDirection = sortModel[0].sort;
            }

            sortColumn = sortColumn === "null" ? "Author" : sortColumn;
            sortDirection = sortDirection === "asc" ? "ASC" : "DESC";

            // Build your API request URL
            const url = API_URL + `book/gridsearch?startRow=${startRow}&endRow=${endRow}&sortColumn=${sortColumn}&sortDirection=${sortDirection}&filter=${filter}`;
            fetch(url)
                .then(response => response.json())
                .then(data => {
                    params.success({ rowData: data, rowCount: data.totalRecords });  // Adjust 'totalRecords' if your API provides it
                })
                .catch(error => {
                    console.error("Error fetching data:", error);
                    params.fail();
                });
        }
    }
};

// Create the grid instance
gridApi = new agGrid.createGrid(document.getElementById('myGrid'), gridOptions);


var savedFilterModel = null;

function clearFilters() {
    gridApi.setFilterModel(null);
}

function saveFilterModel() {

    savedFilterModel = gridApi.getFilterModel();

    var keys = Object.keys(savedFilterModel);
    var savedFilters = keys.length > 0 ? keys.join(", ") : "(none)";

    document.querySelector("#savedFilters").textContent = savedFilters;
}

function restoreFilterModel() {
    gridApi.setFilterModel(savedFilterModel);
}

function destroyFilter() {
    // gridApi.destroyFilter("athlete");
    gridApi.setFilterModel(null);

    document.querySelector("#savedFilters").textContent = '';
}
function currencyFormatter(currency) {
    if (currency === undefined) {
        return "$0";
    }
    else {

        var sansDec = currency.toFixed(0);
        //var formatted = sansDec.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return "$" + `${sansDec}`;
    }
}

gridApi.addEventListener('cellValueChanged', function (event) {
    // get the updated row data
    var updatedRowData = event.data;
    delete updatedRowData.editLink;
    delete updatedRowData.deleteLink;

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedRowData)
    };

    fetch(API_URL + "book/update", options)
        .then(
            response => {
                $('#exampleModal').modal('show');
            }
        ).then(
            html => console.log(html)
        );

});
