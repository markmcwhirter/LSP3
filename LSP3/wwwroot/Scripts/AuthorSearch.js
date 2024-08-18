

    // Column definitions
    const columnDefs = [
    {field: 'authorID', headerName: 'Author ID', cellDataType: 'number', filter: 'agNumberColumnFilter', editable: false },
    {field: 'lastName', headerName: 'Last Name'},
    {field: 'firstName', headerName: 'First Name' },
    {field: 'eMail', flex: 2, headerName: 'EMail' },
    {field: 'notes', headerName: 'Notes' },
    {field: 'editLink', headerName: 'Edit', sortable: false, filter: false, editable: false, cellRenderer: params => { return "<a href='ProfileModify?id=" + params.value + "'>Edit</a>"; } },
    {field: 'deleteLink', headerName: 'Delete', sortable: false, filter: false, editable: false, cellRenderer: params => { return "<a href='DeleteAuthor?id=" + params.value + "'>Delete</a>"; } }
    ];


    let gridApi;

    // Grid options
    const gridOptions = {
        columnDefs: columnDefs,
    pagination: true,
    paginationPageSize: 20,
    rowHeight: 32.5,
    defaultColDef: {
        filter: "agTextColumnFilter",
    floatingFilter: true,
    editable: true,
    sortable: true,
    flex: 1
        },

    rowModelType: 'serverSide',


    serverSideDatasource: {
        getRows: (params) => {
                
    const {startRow, endRow, sortModel} = params.request;
    let sortColumn = "lastName";
    let sortDirection = "ASC";
    let filter = JSON.stringify(params.request.filterModel);

                // Extract sorting information
                if (sortModel.length > 0) {
        sortColumn = sortModel[0].colId;
    sortDirection = sortModel[0].sort;
                }

    sortColumn = sortColumn === "null" ? "AuthorId" : sortColumn;
    sortDirection = sortDirection === "asc" ? "ASC" : "DESC";

    // Build your API request URL
    const url = API_URL + `author/gridsearch?startRow=${startRow}&endRow=${endRow}&sortColumn=${sortColumn}&sortDirection=${sortDirection}&filter=${filter}`;
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
        gridApi.setFilterModel(null);

    document.querySelector("#savedFilters").textContent = '';
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

    fetch(API_URL + "author/update", options)
    .then(
                response => {
        $('#exampleModal').modal('show');
                }
    ).then(
                html => console.log(html)
    );

    });

