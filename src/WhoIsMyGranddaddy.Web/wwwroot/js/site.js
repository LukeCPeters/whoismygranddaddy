(function () {
    let hideTimer;

    function copy(text) {
        if (navigator.clipboard && window.isSecureContext) {
            return navigator.clipboard.writeText(text);
        }
        return new Promise(function (resolve, reject) {
            const ta = document.createElement("textarea");
            ta.value = text;
            ta.style.position = "fixed";
            ta.style.opacity = "0";
            document.body.appendChild(ta);
            ta.select();
            const ok = document.execCommand("copy");
            document.body.removeChild(ta);
            ok ? resolve() : reject();
        });
    }

    function showToast(message) {
        const toast = document.getElementById("toast");
        if (!toast) return;
        toast.querySelector(".toast-msg").textContent = message;
        toast.classList.add("show");
        clearTimeout(hideTimer);
        hideTimer = setTimeout(hideToast, 3000);
    }

    function hideToast() {
        const toast = document.getElementById("toast");
        if (toast) toast.classList.remove("show");
        clearTimeout(hideTimer);
    }

    const ZOOM_MIN = 0.2;
    const ZOOM_MAX = 1;
    const ZOOM_STEP = 0.8;
    let treeZoom = 1;
    let zoomedIdentity = null;

    function layoutTreeConnectors() {
        document.querySelectorAll(".tree li").forEach(function (li) {
            const parents = li.querySelector(":scope > .parents");
            if (parents) {
                const person = parents.querySelector(":scope > .person");
                if (!person) return;
                const couple = parents.getBoundingClientRect();
                const node = person.getBoundingClientRect();
                const shift = ((couple.left + couple.width / 2) - (node.left + node.width / 2)) / treeZoom;
                li.style.setProperty("--mate-shift", Math.max(0, shift).toFixed(2) + "px");
                return;
            }
            const leaf = li.querySelector(":scope > .leaf");
            if (leaf) {
                const liRect = li.getBoundingClientRect();
                const rail = leaf.getBoundingClientRect();
                const shift = ((liRect.left + liRect.width / 2) - rail.left) / treeZoom;
                li.style.setProperty("--mate-shift", Math.max(0, shift).toFixed(2) + "px");
            }
        });
    }

    function centerTreeOnRoot() {
        document.querySelectorAll(".tree").forEach(function (tree) {
            const root = tree.querySelector(":scope > ul > li > .parents");
            if (!root) return;
            const treeRect = tree.getBoundingClientRect();
            const rootRect = root.getBoundingClientRect();
            const rootCenter = (rootRect.left - treeRect.left) + tree.scrollLeft + rootRect.width / 2;
            tree.scrollLeft = rootCenter - tree.clientWidth / 2;
        });
    }

    function applyZoom() {
        document.querySelectorAll(".tree").forEach(function (tree) {
            tree.style.setProperty("--tree-zoom", String(treeZoom));
        });
        document.querySelectorAll(".zoom-level").forEach(function (el) {
            el.textContent = Math.round(treeZoom * 100) + "%";
        });
    }

    function setZoom(value) {
        treeZoom = Math.min(ZOOM_MAX, Math.max(ZOOM_MIN, value));
        applyZoom();
        layoutTreeConnectors();
        centerTreeOnRoot();
    }

    function fitTree() {
        const tree = document.querySelector(".tree");
        if (!tree || tree.scrollWidth === 0) return;
        setZoom(treeZoom * tree.clientWidth / tree.scrollWidth);
    }

    function updateZoomControls() {
        const bar = document.querySelector(".tree-zoom");
        if (!bar) return;
        const tree = document.querySelector(".tree");
        if (!tree) { bar.hidden = true; return; }
        const tooWide = tree.scrollWidth > tree.clientWidth + 1;
        const tooTall = tree.getBoundingClientRect().height > window.innerHeight * 0.7;
        bar.hidden = !(tooWide || tooTall || treeZoom < ZOOM_MAX);
    }

    function renderTree() {
        const tree = document.querySelector(".tree");
        const identity = tree ? tree.getAttribute("data-identity") : null;
        if (identity !== zoomedIdentity) {
            treeZoom = 1;
            zoomedIdentity = identity;
        }
        applyZoom();
        layoutTreeConnectors();
        centerTreeOnRoot();
        updateZoomControls();
    }

    let layoutTimer;
    function scheduleLayout() {
        clearTimeout(layoutTimer);
        layoutTimer = setTimeout(function () {
            layoutTreeConnectors();
            updateZoomControls();
        }, 50);
    }

    document.addEventListener("DOMContentLoaded", renderTree);
    document.addEventListener("htmx:afterSwap", renderTree);
    window.addEventListener("resize", scheduleLayout);
    if (document.fonts && document.fonts.ready) {
        document.fonts.ready.then(renderTree);
    }
    renderTree();

    document.addEventListener("click", function (e) {
        const copyBtn = e.target.closest(".copy-id");
        if (copyBtn) {
            const id = copyBtn.getAttribute("data-id");
            copy(id).then(
                function () { showToast("ID " + id + " copied to clipboard"); },
                function () { showToast("Couldn't copy — copy it manually"); }
            );
            return;
        }
        if (e.target.closest(".zoom-out")) { setZoom(treeZoom * ZOOM_STEP); return; }
        if (e.target.closest(".zoom-in")) { setZoom(treeZoom / ZOOM_STEP); return; }
        if (e.target.closest(".zoom-fit")) { fitTree(); return; }
        if (e.target.closest(".toast-close")) hideToast();
    });
})();
