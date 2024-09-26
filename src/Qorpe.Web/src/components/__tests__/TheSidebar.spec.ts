import { describe, it, expect } from 'vitest'

import { mount } from '@vue/test-utils'
import TheSidebar from '../TheSidebar.vue'

describe('HelloWorld', () => {
    it('renders properly', () => {
        const wrapper = mount(TheSidebar, { props: { rail: false } })
        expect(wrapper.text()).toContain('Hello Vitest')
    })
})
