import { createI18n } from 'vue-i18n'

import enUS from '@/locales/en-US.json'
import trTR from '@/locales/tr-TR.json'

// Type-define 'en-US' as the master schema for the resource
type MessageSchema = typeof enUS

const i18n = createI18n<[MessageSchema], 'en-US' | 'tr-TR'>({
    locale: import.meta.env.VITE_I18N_LOCALE || 'en-US',
    fallbackLocale: import.meta.env.VITE_I18N_FALLBACK_LOCALE || 'en-US',
    messages: {
        'en-US': enUS,
        'tr-TR': trTR
    }
})

export default i18n;
