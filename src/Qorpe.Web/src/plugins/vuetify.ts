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
            density: "comfortable"
        },
        VBtn: {
            // density: "comfortable"
        },
        VList: {
            density: "comfortable"
        },
        VAppBar: {
            density: "comfortable"
        },
        VAutocomplete: {
            density: "comfortable"
        },
        VSelect: {
            density: "comfortable"
        },
        VCombobox: {
            density: "comfortable"
        }
    }
})
