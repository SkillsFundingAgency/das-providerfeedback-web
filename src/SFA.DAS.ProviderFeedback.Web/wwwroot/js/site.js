// FEEDBACK GRAPH
// Generates the html to display
// feedback graph and populates the target

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}

function FeedbackGraph(table) {
    this.table = table
    this.target = this.table.dataset.target
    this.hideLegend = this.table.dataset.hideLegend === "true"
    this.label = this.table.dataset.label || "people"
    this.rows = this.table.querySelectorAll("tbody tr")
}

FeedbackGraph.prototype.init = function () {
    if (!document.getElementById(this.target)) {
        return
    }

    var that = this
    var rowCount = 0
    var legendlistHtml
    var graphHtml = document.createElement("div")
    graphHtml.className = "app-graph"

    var graphList = document.createElement("ul")
    graphList.className = "app-graph__list"

    var caption = this.table.querySelector("caption")
    var categoryHtml

    nodeListForEach(this.rows, function (row) {
        if (rowCount === 0) {
            legendlistHtml = that.legendHtml(row)
        }
        graphList.appendChild(that.graphRow(row))
        rowCount++
    });

    if (!this.hideLegend) {
        graphHtml.appendChild(legendlistHtml)
    }

    if (caption) {
        categoryHtml = document.createElement("h2")
        categoryHtml.className = "govuk-caption-l app-graph__category"
        categoryHtml.textContent = caption.textContent
        graphHtml.appendChild(categoryHtml)
    }

    graphHtml.appendChild(graphList)
    document.getElementById(this.target).appendChild(graphHtml)
}

FeedbackGraph.prototype.legendHtml = function (row) {
    var legendList = document.createElement('ul')
    legendList.className = "app-graph-key"
    var dataCells = row.querySelectorAll("td")
    var cellCount = 0

    if (this.hideLegend) {
        return ''
    }

    nodeListForEach(dataCells, function (dataCell) {
        if (isNaN(dataCell.textContent)) {

            var legendListItem = document.createElement("li")
            legendListItem.className = "app-graph-key__list-item app-graph-key__list-item--colour-" + (cellCount + 1)
            legendListItem.textContent = dataCell.dataset.label

            legendList.appendChild(legendListItem)
            cellCount++
        }
    });
    return legendList
}

FeedbackGraph.prototype.graphRow = function (row) {
    var that = this
    var questionText = row.querySelector("th").textContent
    var dataCells = row.querySelectorAll("td")
    var graphRowHtml = document.createElement('li')
    var barsHtml = document.createElement("div")
    var totalAsked = 0
    var barCount = 0

    graphRowHtml.className = "app-graph__list-item"
    barsHtml.className = "app-graph__chart-wrap"

    nodeListForEach(dataCells, function (dataCell) {
        if (isNaN(dataCell.textContent)) {
            var barHtml = that.barHtml(dataCell, barCount + 1)
            if (barHtml !== undefined) {
                barsHtml.appendChild(barHtml)
            }
            barCount++
        } else {
            totalAsked = dataCell.textContent
        }
    });

    var caption = document.createElement('span')
    caption.className = "app-graph__caption"
    caption.textContent = "(selected by " + totalAsked + " " + this.label + ")"
    var heading = document.createElement('h3')
    heading.className = "app-graph__label"
    heading.textContent = questionText
    if (totalAsked > 0) {
        heading.appendChild(caption)
    }

    graphRowHtml.appendChild(heading)
    graphRowHtml.appendChild(barsHtml)

    return (graphRowHtml)
}

FeedbackGraph.prototype.barHtml = function (dataCell, barCount) {
    var percentage = parseFloat(dataCell.textContent.slice(0, -1));
    var span1 = document.createElement('span')
    span1.textContent = percentage + "%"
    span1.className = "app-graph__figure"

    var span2 = document.createElement('span')
    span2.className = "app-graph__bar-value app-graph__bar-value--colour-" + barCount
    span2.style.width = percentage + "%"
    span2.title = dataCell.dataset.title
    span2.tabIndex = 0
    span2.appendChild(span1)

    var span3 = document.createElement('span')
    span3.className = "app-graph__bar"
    span3.appendChild(span2)

    var span4 = document.createElement('span')
    span4.className = "app-graph__chart"
    span4.appendChild(span3)

    return (span4)

}

// SHOW/HIDE PANELS

function ShowHidePanels(container) {
    this.container = container
    this.panels = this.container.querySelectorAll('.app-show-hide-panel')
    this.hideClass = 'app-show-hide-panel__hidden'
}

ShowHidePanels.prototype.init = function () {
    var panelData = []
    var that = this
    nodeListForEach(this.panels, function (panel) {
        var panelObj = {
            id: panel.id,
            label: panel.dataset.panelLabel
        }
        panelData.push(panelObj)
        that.hidePanel(panel)
    })
    nodeListForEach(this.panels, function (panel) {
        panel.prepend(that.panelNav(panel.id, panelData))
    })
    this.showPanel(this.panels[0])
}

ShowHidePanels.prototype.panelNav = function (panelId, panelData) {
    var that = this
    var buttonWrap = document.createElement('div')
    buttonWrap.className = "govuk-button-group app-show-hide-panel__buttons"
    var filteredData = panelData.filter(function (item) {
        return item.id !== panelId
    })
    filteredData.forEach((item) => {
        buttonWrap.appendChild(that.showHideButton(item))
    });
    return buttonWrap
}

ShowHidePanels.prototype.showHideButton = function (item) {
    var button = document.createElement('a')
    button.className = 'govuk-button govuk-button--secondary govuk-!-font-size-16'
    button.textContent = 'Change to ' + item.label + ' view'
    button.href = '#' + item.id
    button.addEventListener('click', this.handleButtonClick.bind(this))
    return button
}

ShowHidePanels.prototype.handleButtonClick = function (e) {
    var that = this
    var targetPanel = e.target.hash.substring(1)
    nodeListForEach(this.panels, function (panel) {
        if (panel.id !== targetPanel) {
            that.hidePanel(panel)
        } else {
            that.showPanel(panel)
        }
    })
    e.preventDefault()
}

ShowHidePanels.prototype.hidePanel = function (panel) {
    panel.classList.add(this.hideClass)
}

ShowHidePanels.prototype.showPanel = function (panel) {
    panel.classList.remove(this.hideClass)
}

var showHidePanels = document.querySelectorAll('[data-show-hide-panels]');
nodeListForEach(showHidePanels, function (showHidePanel) {
    new ShowHidePanels(showHidePanel).init();
});

var feedbackGraphs = document.querySelectorAll('[data-feedback-graph]');
nodeListForEach(feedbackGraphs, function (feedbackGraph) {
    new FeedbackGraph(feedbackGraph).init();
});
