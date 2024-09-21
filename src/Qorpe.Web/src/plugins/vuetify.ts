// Styles
import 'vuetify/styles'
// import 'material-design-icons-iconfont/dist/material-design-icons.css'
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
            variant: 'underlined'
        },
        VBtn: {
            variant: 'text'
        },
    }
})
