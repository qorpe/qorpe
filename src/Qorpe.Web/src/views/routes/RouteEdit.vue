<script setup lang="ts">
//#region Import(s)
import { ref, computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useRoutesStore } from '@/stores/routes'
import type { IQueryParameter, IRoute, ITransform, ITransformGroup } from '@/interfaces'
//#endregion

//#region General
const route = useRoute()
const routesStore = useRoutesStore()
const _route: IRoute = routesStore.getSelected(route.params.routeId.toString())
const panel = ref<string[]>(["general"])
//#endregion

//#region Match
const httpMethods: string[] = [
    "GET",
    "POST",
    "PUT",
    "DELETE",
    "PATCH",
    "OPTIONS",
    "HEAD",
    "CONNECT",
    "TRACE"
];

const yesNoList: boolean[] = [true, false]

const queryParameter = ref<IQueryParameter>({} as IQueryParameter)

const headerQueryParameter = ref([
    { title: 'Name', key: 'name' },
    { title: 'Values', key: 'values' },
    { title: 'Mode', key: 'mode' },
    { title: 'IsCaseSensitive', key: 'isCaseSensitive' },
    { title: 'Actions', key: 'actions' }
])

const drawer = ref(true)
const rail = ref(true)
//#endregion

//#region Metadata(s)
const keyMetadata = ref<string | null>()
const valueMetadata = ref<string | null>()

const addOrUpdateMetada = () => {
    if (!keyMetadata.value || !valueMetadata.value) return
    _route.metadata[keyMetadata.value] = valueMetadata.value
}

const removeMetada = (key: string) => {
    delete _route.metadata[key];
}

const setMetada = (key: string, value: string) => {
    if (!key || !value) return
    keyMetadata.value = key
    valueMetadata.value = value
}
//#endregion

//#region Transform(s)
const keyTransformList: string[] = [
    'ClientCert',
    'Forwarded',
    'HttpMethodChange',
    'PathPattern',
    'PathRemovePrefix',
    'PathSet',
    'QueryRemoveParameter',
    'QueryRouteParameter',
    'QueryValueParameter',
    'RequestHeader',
    'RequestHeaderOriginalHost',
    'RequestHeaderRemove',
    'RequestHeaderRouteValue',
    'RequestHeadersAllowed',
    'RequestHeadersCopy',
    'ResponseHeader',
    'ResponseHeaderRemove',
    'ResponseHeadersAllowed',
    'ResponseHeadersCopy',
    'ResponseTrailer',
    'ResponseTrailerRemove',
    'ResponseTrailersAllowed',
    'ResponseTrailersCopy',
    'Set',
    'X-Forwarded',

    'Set',
    'Append',
    'When',

    'For',
    'Proto',
    'Host',
    'Prefix',

    'ForFormat',
    'ByFormat',
    'Action',
]

const keyTransform = ref<string>()
const valueTransform = ref<string>()
const groupTransform = ref<ITransformGroup>()

const getTransformGroups = computed(() => {
    return (transforms: ITransform[]): ITransformGroup[] => {
        return transforms.map((_, index) => {
            return {
                index,
                description: `Group - ${index + 1}`
            } as ITransformGroup
        });
    };
});

const addOrUpdateTransform = () => {
    if (!keyTransform.value || !valueTransform.value) return
    if (groupTransform.value && groupTransform.value.index < _route.transforms.length)
        _route.transforms[groupTransform.value.index][keyTransform.value] = valueTransform.value
    else {
        const newTransform: ITransform = {
            [keyTransform.value]: valueTransform.value
        };
        _route.transforms.push(newTransform)
    }
}

const removeTransform = (key: string, index: number) => {
    if (!key || index >= _route.transforms.length) return;

    delete _route.transforms[index][key];

    if (Object.keys(_route.transforms[index]).length === 0) {
        _route.transforms.splice(index, 1);
        groupTransform.value = undefined
        keyTransform.value = undefined
        valueTransform.value = undefined
    }
};

const setTransform = (key: string, value: string, index: number) => {
    if (!key || !value) return

    keyTransform.value = key
    valueTransform.value = value

    if (!groupTransform.value)
        groupTransform.value = {} as ITransformGroup

    groupTransform.value.index = index
    groupTransform.value.description = `Group - ${index + 1}`
}
//#endregion
</script>

<template>
    <v-row dense class="mb-5">
        <v-col cols="12" md="4">
            <v-card variant="tonal" color="surface-variant" append-icon="mdi-tag-outline" :subtitle="_route?.routeId">
                <template v-slot:title>
                    Route
                </template>
            </v-card>
        </v-col>

        <v-col cols="12" md="3">
            <v-btn variant="tonal" class=" me-2"
                @click="panel = ['general', 'policies', 'match', 'metadata', 'transforms']">
                All
            </v-btn>
            <v-btn variant="tonal" class="" @click="panel = []">
                None
            </v-btn>
        </v-col>
    </v-row>

    <v-expansion-panels v-model="panel" multiple focusable flat>
        <v-expansion-panel class="mb-2" value="general">
            <v-expansion-panel-title class="border">General</v-expansion-panel-title>
            <v-expansion-panel-text>
                <v-row dense class="mt-1">
                    <v-col cols="12" md="6">
                        <v-text-field v-model="_route.name" label="Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field v-model="_route.description" label="Description"></v-text-field>
                    </v-col>
                </v-row>
                <v-row dense>
                    <v-col cols="12" md="3">
                        <v-autocomplete label="Cluster Id"></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-text-field v-model="_route.timeout" label="Timeout"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-text-field v-model="_route.order" type="number" label="Order"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-text-field v-model="_route.maxRequestBodySize" type="number"
                            label="Max Request Body Size"></v-text-field>
                    </v-col>
                </v-row>
            </v-expansion-panel-text>
        </v-expansion-panel>

        <v-expansion-panel class="mb-2" value="policies">
            <v-expansion-panel-title class="border">Policies</v-expansion-panel-title>
            <v-expansion-panel-text>
                <v-row dense class="mt-1">
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="_route.rateLimiterPolicy" label="Rate-Limiter Policy"></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="_route.timeoutPolicy" label="Timeout Policy"></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="_route.corsPolicy" label="Cors Policy"></v-autocomplete>
                    </v-col>
                </v-row>
                <v-row dense>
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="_route.authorizationPolicy"
                            label="Authorization Policy"></v-autocomplete>
                    </v-col>
                    <v-col cols="12" md="4">
                        <v-autocomplete v-model="_route.outputCachePolicy" label="Output-Cache Policy"></v-autocomplete>
                    </v-col>
                </v-row>
            </v-expansion-panel-text>
        </v-expansion-panel>

        <v-expansion-panel class="mb-2" value="match">
            <v-expansion-panel-title class="border">Match</v-expansion-panel-title>
            <v-expansion-panel-text>
                <v-row dense class="mt-1">
                    <v-col cols="12" md="6">
                        <v-select v-model="_route.match.methods" :items="httpMethods" label="Methods" multiple
                            clearable></v-select>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-combobox v-model="_route.match.hosts" :items="_route.match.hosts" label="Hosts" multiple
                            clearable></v-combobox>
                    </v-col>
                </v-row>
                <v-row dense>
                    <v-col cols="12" md="6">
                        <v-text-field v-model="_route.match.path" label="Path"></v-text-field>
                    </v-col>
                </v-row>
                <v-row dense>
                    <v-col cols="12" md="6">
                        <v-combobox v-model="_route.match.queryParameters" return-object readonly item-title="name"
                            label="Query Parameters" multiple>
                            <template v-slot:append-inner>
                                <v-dialog max-width="90%">
                                    <template v-slot:activator="{ props: activatorProps }">
                                        <v-btn v-bind="activatorProps" size="small" variant="text">
                                            <v-icon>mdi-pencil-outline</v-icon>
                                        </v-btn>
                                    </template>
                                    <template v-slot:default="{ isActive }">
                                        <v-card>
                                            <v-card-text>
                                                <v-row dense>
                                                    <v-col cols="12" md="7">
                                                        <v-data-table :headers="headerQueryParameter"
                                                            hide-default-footer item-value="name">
                                                            <template v-slot:top>
                                                                <!-- <v-btn variant="tonal">Add</v-btn> -->
                                                            </template>
                                                        </v-data-table>
                                                        <v-list lines="one">
                                                            <v-list-item variant="plain" color="surface-variant"
                                                                v-for="(queryParameter, i) in _route.match.queryParameters"
                                                                :key="i">
                                                                <template v-slot:title>
                                                                    {{ queryParameter.name }}
                                                                    <v-chip>
                                                                        Mode: {{ queryParameter.mode }}
                                                                    </v-chip>
                                                                    <v-chip>
                                                                        IsCase-Sensitive: {{ queryParameter.mode }}
                                                                    </v-chip>
                                                                </template>
                                                                <template v-slot:subtitle>
                                                                    {{ queryParameter.values.join(', ') }}
                                                                </template>
                                                                <template v-slot:append>
                                                                    <v-btn color="grey-lighten-1"
                                                                        icon="mdi-pencil-outline"
                                                                        variant="text"></v-btn>
                                                                    <v-btn color="grey-lighten-1"
                                                                        icon="mdi-delete-outline"
                                                                        variant="text"></v-btn>
                                                                </template>
                                                            </v-list-item>
                                                        </v-list>
                                                    </v-col>
                                                    <v-col cols="12" md="5">
                                                        <v-card variant="plain" color="surface-variant">
                                                            <v-card-text>
                                                                <v-text-field v-model="queryParameter.name"
                                                                    label="Name"></v-text-field>
                                                                <v-combobox v-model="queryParameter.values"
                                                                    :items="queryParameter.values" label="Values"
                                                                    multiple clearable tile></v-combobox>
                                                                <v-select v-model:number="queryParameter.mode"
                                                                    label="Mode" multiple clearable tile></v-select>
                                                                <v-select v-model:bool="queryParameter.isCaseSensitive"
                                                                    :items="yesNoList" label="Is Case-Sensitive"
                                                                    clearable tile class="mb-3"></v-select>
                                                                <v-btn @click="rail = true" block variant="tonal"
                                                                    class="">
                                                                    Add or Update Query Parameter
                                                                </v-btn>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                            </v-card-text>
                                            <v-card-actions>
                                                <v-spacer></v-spacer>
                                                <v-btn text="Close" @click="isActive.value = false"
                                                    icon="mdi-close"></v-btn>
                                            </v-card-actions>
                                        </v-card>
                                    </template>
                                </v-dialog>
                            </template>
                        </v-combobox>
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-combobox v-model="_route.match.headers" return-object item-title="name" label="Headers"
                            multiple clearable></v-combobox>
                    </v-col>
                </v-row>
            </v-expansion-panel-text>
        </v-expansion-panel>

        <v-expansion-panel class="mb-2" value="metadata">
            <v-expansion-panel-title class="border">Metadata</v-expansion-panel-title>
            <v-expansion-panel-text>
                <v-row dense>
                    <v-col cols="12" md="7">
                        <v-list lines="one">
                            <v-list-item v-for="(value, key) in _route.metadata" :key="key" :title="key"
                                :subtitle="value">
                                <template v-slot:append>
                                    <v-btn @click="setMetada(key.toString(), value)" color="grey-lighten-1"
                                        icon="mdi-pencil-outline" variant="text"></v-btn>
                                    <v-btn @click="removeMetada(key.toString())" color="grey-lighten-1"
                                        icon="mdi-delete-outline" variant="text"></v-btn>
                                </template>
                            </v-list-item>
                        </v-list>
                    </v-col>
                    <v-col cols="12" md="5">
                        <v-card variant="plain" color="surface-variant">
                            <v-card-text>
                                <v-text-field v-model="keyMetadata" label="Key"></v-text-field>
                                <v-textarea v-model="valueMetadata" label="Value" tile rows="2"></v-textarea>
                                <v-btn @click="addOrUpdateMetada" block variant="tonal" class="">
                                    Add or Update Metadata
                                </v-btn>
                            </v-card-text>
                        </v-card>
                    </v-col>
                </v-row>
            </v-expansion-panel-text>
        </v-expansion-panel>

        <v-expansion-panel value="transforms">
            <v-expansion-panel-title class="border">Transforms</v-expansion-panel-title>
            <v-expansion-panel-text>
                <v-row dense justify="space-between">
                    <v-col cols="12" md="7">
                        <template v-for="(item, i) in _route.transforms" :key="i">
                            <v-list lines="one">
                                <v-list-subheader>
                                    Group - {{ i + 1 }}
                                </v-list-subheader>
                                <v-list-item v-for="(value, key) in item" :key="key" :title="key" :subtitle="value">
                                    <template v-slot:append>
                                        <v-btn @click="setTransform(key.toString(), value, i)" color="grey-lighten-1"
                                            icon="mdi-pencil-outline" variant="text"></v-btn>
                                        <v-btn @click="removeTransform(key.toString(), i)" color="grey-lighten-1"
                                            icon="mdi-delete-outline" variant="text"></v-btn>
                                    </template>
                                </v-list-item>
                            </v-list>
                            <v-divider style="border-style: dashed;"
                                v-if="i < _route.transforms.length - 1"></v-divider>
                        </template>
                    </v-col>
                    <v-col cols="12" md="5">
                        <v-card variant="plain" color="surface-variant">
                            <v-card-text>
                                <v-autocomplete v-model:model-value="groupTransform" return-object label="Group"
                                    item-title="description" item-value="index"
                                    :items="getTransformGroups(_route.transforms)" clearable></v-autocomplete>
                                <v-autocomplete v-model="keyTransform" :items="keyTransformList" label="Key"
                                    tile></v-autocomplete>
                                <v-textarea v-model="valueTransform" label="Value" tile rows="2"></v-textarea>
                                <v-btn @click="addOrUpdateTransform" block variant="tonal" class="">
                                    Add or Update Transform
                                </v-btn>
                            </v-card-text>
                        </v-card>
                    </v-col>
                </v-row>
            </v-expansion-panel-text>
        </v-expansion-panel>
    </v-expansion-panels>
</template>