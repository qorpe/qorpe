// Styles
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'

// Vuetify
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

export default createVuetify({
    icons: {
        defaultSet: 'mdi',
    },
    components,
    directives,
    defaults: {
        VTextField: {
            density: "compact"
        },
        VBtn: {
            // density: "comfortable"
        },
        VList: {
            density: "compact"
        },
        VListItem: {
            density: "compact"
        },
        VAppBar: {
            density: "compact"
        },
        VAutocomplete: {
            density: "compact"
        },
        VSelect: {
            density: "compact"
        },
        VCombobox: {
            density: "compact"
        }
    }
})
