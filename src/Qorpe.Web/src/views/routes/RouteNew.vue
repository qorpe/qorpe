<template>
    <v-form ref="form" @submit.prevent="submit" class="mt-5">
        <v-expansion-panels flat v-model="panel">
            <v-expansion-panel class="mb-1">
                <v-expansion-panel-title color="grey-lighten-4">General</v-expansion-panel-title>
                <v-expansion-panel-text>
                    <v-row class="mt-3">
                        <v-col cols="12" md="4">
                            <v-text-field label="Route Id" readonly :error-messages="errors.userName"
                                required></v-text-field>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Cluster Id"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field label="Order" type="number" clearable></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="4">
                            <v-text-field label="Maximum Request Body Size" type="number" clearable></v-text-field>
                        </v-col>
                    </v-row>
                </v-expansion-panel-text>
            </v-expansion-panel>
            <v-expansion-panel class="mb-1">
                <v-expansion-panel-title color="grey-lighten-4">Matches</v-expansion-panel-title>
                <v-expansion-panel-text>
                    <v-row class="mt-3">
                        <v-col cols="12" md="6">
                            <v-select v-model="selectedMethods" :items="['GET', 'POST', 'PUT', 'DELETE']"
                                label="Methods" multiple chips clearable />
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-combobox v-model="selectedHosts" :items="hosts" label="Hosts" hide-selected multiple
                                chips clearable />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="4">
                            <v-text-field label="Path"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="6">
                        </v-col>
                        <v-col cols="12" md="6">
                        </v-col>
                    </v-row>
                </v-expansion-panel-text>
            </v-expansion-panel>
            <v-expansion-panel>
                <v-expansion-panel-title color="grey-lighten-4">Policies</v-expansion-panel-title>
                <v-expansion-panel-text>
                    <v-row class="mt-3">
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Authorization Policy"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Cors Policy"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Rate-Limiter Policy"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Output-Cache Policy"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-autocomplete clearable label="Timeout Policy"
                                :items="['California', 'Colorado', 'Florida', 'Georgia', 'Texas', 'Wyoming']"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field v-model="timeoutValue" label="Timeout" :rules="[timeoutRule]"
                                placeholder="HH:MM:SS" @blur="formatTimeout"></v-text-field>
                        </v-col>
                    </v-row>
                </v-expansion-panel-text>
            </v-expansion-panel>
        </v-expansion-panels>
        <v-btn :loading="loading" class="mt-2 " type="submit" block>Create</v-btn>
    </v-form>
</template>

<!-- <script setup lang="ts" ></script> -->

<script>
export default {
    data: () => ({
        panel: 0,
        loading: false,
        userName: '',
        email: '',
        password: '',
        timeoutValue: '',
        selectedMethods: [],
        selectedHosts: [],
        hosts: ['example.com', 'api.example.com'],
        timeoutRule: v => {
            const regex = /^\d{2}:\d{2}:\d{2}$/; // HH:MM:SS format
            return regex.test(v) || 'Invalid timeout format (HH:MM:SS)';
        },
        errors: {
            userName: [],
            email: [],
            password: [],
        },
    }),

    methods: {
        formatTimeout() {
            // Boş değer durumunda işlemi durdur
            if (!this.timeoutValue) return;

            // Girilen değeri boşluklardan arındır
            let value = this.timeoutValue.trim();

            // Regex ile kontrol et ve formatla
            const regex = /^(\d{1,2}):(\d{2}):(\d{2})$/;
            const match = value.match(regex);

            if (!match) {
                // Eğer format yanlışsa, kullanıcıdan yeni format isteyebiliriz
                this.timeoutValue = ''; // Hatalıysa temizle
                return;
            }

            let hours = parseInt(match[1]);
            let minutes = parseInt(match[2]);
            let seconds = parseInt(match[3]);

            // Saat, dakika ve saniye sınırlarını kontrol et
            if (hours > 99) hours = 99; // Maksimum 99 saat
            if (minutes > 59) minutes = 59; // Maksimum 59 dakika
            if (seconds > 59) seconds = 59; // Maksimum 59 saniye

            // Yeni formatlanmış değeri ayarla
            this.timeoutValue =
                `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
        },
        async submit() {
            this.loading = true;
            this.clearErrors();

            try {
                // Backend API çağrısı
                const response = await this.checkApi({
                    userName: this.userName,
                    email: this.email,
                    password: this.password,
                });

                // API'den gelen validasyon hatalarını işliyoruz
                if (response && response.errors) {
                    this.processErrors(response.errors);
                } else {
                    alert("Form submitted successfully!");
                }
            } catch (error) {
                console.error("API call failed:", error);
            } finally {
                this.loading = false;
            }
        },

        // Backend'e tüm form verilerini gönderiyoruz
        async checkApi(formData) {
            return new Promise((resolve) => {
                setTimeout(() => {
                    const errors = {
                        userName: formData.userName === ''
                            ? ['User name is required.', 'User name must be unique.']
                            : [],
                        email: formData.email === ''
                            ? ['Email is required.']
                            : !this.isValidEmail(formData.email)
                                ? ['Email must be valid.']
                                : [],
                        password: formData.password.length < 6
                            ? ['Password must be at least 6 characters.', 'Password must contain a number.']
                            : [],
                    };
                    resolve({ errors });
                }, 1000);
            });
        },

        processErrors(errors) {
            // Backend'den gelen hataları işliyoruz
            this.errors = errors;
        },

        clearErrors() {
            // Hata mesajlarını temizliyoruz
            this.errors = {
                userName: [],
                email: [],
                password: [],
            };
        },

        // Email format kontrol fonksiyonu
        isValidEmail(email) {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailPattern.test(email);
        },
    },
};
</script>

<style scoped>
:deep(.v-expansion-panel-text__wrapper) {
    padding: 0 !important;
}
</style>