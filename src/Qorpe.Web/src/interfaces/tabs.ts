export interface ITab {
    type: 'route' | 'cluster' | 'settings' | 'overview';
    to: string;
    label: string;
    icon?: string;
}