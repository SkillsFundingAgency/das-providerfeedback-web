
// Function to create a cookie
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

 // Function to read a cookie
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

//function get env
function getEnvFromHost() {
    var host = window.location.host;
    console.log(host);
    var env = "";

    if (host.includes("at-pas")) {
        env = "AT";
    } else if (host.includes("test-pas")) {
        env = "TEST";
    } else if (host.includes("test2-pas")) {
        env = "TEST2";
    } else if (host.includes("pp-pas")) {
        env = "PP";
    }

    return env;
}

// Function to toggle between graph and table panels and save preference in a cookie
function toggleTables(panelId) {
    // Select all graph panels
    var graphPanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="graph"]');
    // Select all table panels
    var tablePanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="table and accessible"]');

    // Toggle visibility for all graph panels
    var graphVisible = false;
    for (var i = 0; i < graphPanels.length; i++) {
        graphPanels[i].classList.toggle("app-show-hide-panel__hidden");
        if (!graphPanels[i].classList.contains("app-show-hide-panel__hidden")) {
            graphVisible = true;
        }
    }

    // Toggle visibility for all table panels
    for (var i = 0; i < tablePanels.length; i++) {
        tablePanels[i].classList.toggle("app-show-hide-panel__hidden");
    }

    var analyticsConsentName = "AnalyticsConsent" + getEnvFromHost();
    var analyticsConsentChoice = getCookie(analyticsConsentName);

    // Save user preference in a cookie conditional to the analytics consent
    if (analyticsConsentChoice === "true") {
        setCookie('viewPreference', graphVisible ? 'graph' : 'table', 1);
    }
}

function applyUserPreference() {
    var preference = getCookie('viewPreference');
    if (preference === 'table') {
        // Show table panels and hide graph panels
        var graphPanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="graph"]');
        var tablePanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="table and accessible"]');

        for (var i = 0; i < graphPanels.length; i++) {
            graphPanels[i].classList.add("app-show-hide-panel__hidden");
        }

        for (var i = 0; i < tablePanels.length; i++) {
            tablePanels[i].classList.remove("app-show-hide-panel__hidden");
        }
    } else {
        // Default behavior: show graph panels and hide table panels
        var graphPanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="graph"]');
        var tablePanels = document.querySelectorAll('.app-show-hide-panel[data-panel-label="table and accessible"]');

        for (var i = 0; i < graphPanels.length; i++) {
            graphPanels[i].classList.remove("app-show-hide-panel__hidden");
        }

        for (var i = 0; i < tablePanels.length; i++) {
            tablePanels[i].classList.add("app-show-hide-panel__hidden");
        }
    }
}

// Apply user preference when the page loads
document.addEventListener('DOMContentLoaded', function () {
    applyUserPreference();
});

