(function () {

    function Layout() {

        // ==== private 
        // =========================================
        var self = this;
        var $tabsContent = $('#appLayoutTabsContent');
        var $tabSelect = $('#appLayoutTabSelect');
        var $lastTabActive = null;


        // ==== private - handles
        // =========================================
        function handleTabSelectChange (e) {

            var tabId = $(e.target).val();

            $lastTabActive = $tabsContent.find('.app-active');
            $lastTabActive.removeClass('app-active');
            $('#' + tabId).addClass('app-active');            

            window.location.hash = tabId;
        }

        function handleDataTabLoad(e) {

            var $el = e.target.tagName === 'A' ? $(e.target) : $(e.target).closest('a');
            var url = $el.data('tab-load');
            var title = $el.data('tab-title');
            var tabId = $el.attr('href').replace('#', '');
            var managerComponent = $el.data('tab-manager');

            self.loadTab(url, tabId, title, managerComponent);

            if (isMobileLayout())
                toggleMenu(false);
        }


        // ==== private - functions
        // =========================================
        function init () {

            $tabSelect.change(handleTabSelectChange);            
            $(window).resize(checkIsMobile);

            $(document).on('click', '[data-tab-load]', handleDataTabLoad);

            enableInputUpperCase();
            checkIsMobile();
        }

        function isMobileLayout() {

            var isMobile = $(document.body).is('.mobile-layout');

            return isMobile;
        }

        function checkIsMobile() {

            var $body = $(document.body);
            var isMobile = window.matchMedia('screen and (max-width: 767px)').matches;                 

            if (isMobile)
                $body.addClass('mobile-layout');
            else
                $body.removeClass('mobile-layout');
        }

        function toggleMenu(openOrClose) {

            var $btnMenu = $('.navbar-header .bars');

            if (openOrClose === undefined)
                $btnMenu.click();
            else {

                isOpened = $('body.overlay-open').length > 0;

                if (openOrClose && !isOpened)
                    $btnMenu.click();

                if (!openOrClose && isOpened)
                    $btnMenu.click();
            }
        }

        function enableInputUpperCase() {

            $(document).on('change', 'input:not([type="password"])', function (e) {

                e.target.value = e.target.value.toUpperCase();
            });
        }

        // ==== public 
        // =========================================
        this.loadTab = function (url, tabId, title, managerComponent) {

            var $tabContent = $('#' + tabId);

            if ($tabContent.length !== 0) {
                self.activeTab(tabId);
                return;
            }

            $.get(url)
                .done(function (html) {
                   
                    if ($tabContent.length === 0) {

                        $tabContent = $('<div class="app-tab" id="' + tabId + '"></div>');
                        $tabContent.appendTo($tabsContent);
                        $tabSelect.append('<option value="' + tabId + '">' + title + '</option>');
                        $tabSelect.selectpicker('refresh');
                    }

                    $tabContent.html(html);

                    if (typeof managerComponent === 'string' && managerComponent.length > 0)
                        new app[managerComponent]({ rootElement: '#' + tabId });

                    self.activeTab(tabId);
                })
                .fail(function () {

                    app.notify.error('Falha ao baixar página. Verifique sua conexão com a internet.');
                });
        };

        this.activeTab = function (tabId) {
            
            $tabSelect.val(tabId);
            $tabSelect.selectpicker('refresh').change();
        };

        this.closeTab = function (tabId) {

            $('#' + tabId).remove();
            $tabSelect.find('[value="' + tabId + '"]').remove();

            if ($lastTabActive !== null) {

                var lastTabId = $lastTabActive.attr('id');
                self.activeTab(lastTabId);

                $lastTabActive = null;
            }
        };        


        // ==== init 
        // =========================================
        init();
    }

    $(document).ready(function () { 

        app.layout = new Layout();
    });
})();