class JSFilter {
    constructor(table, feedback) {
        this.table = table;
        this.feedback = feedback;
    }

    FilterBy(value, colindex) {
        var rows = this.table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");
        var count = 0;
        for (var i = 0; i < rows.length; i++) {
            var cell = rows[i].getElementsByTagName("td")[colindex];

            if (value != "") {
                if (rows[i].style.display == "") {
                    if (cell.textContent.includes(value.trim().toUpperCase())) {
                        rows[i].style.display = "";
                        count += 1;
                    } else {
                        rows[i].style.display = "none";
                    }
                }
            } else {
                rows[i].style.display = "";
            }
        }

        this.feedback.innerHTML = count + " Registro(s)"
    }
}