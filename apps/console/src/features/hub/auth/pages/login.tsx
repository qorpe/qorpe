import { useState } from "react";
import { Card, Form, Input, Button, Typography, Alert } from "antd";
import { useSessionStore } from "@/app/stores/session-store";

export default function Login() {
    const login = useSessionStore((s) => s.login);
    const [loading, setLoading] = useState(false);
    const [err, setErr] = useState<string | null>(null);

    const onFinish = async (v: { usernameOrEmail: string; password: string }) => {
        setErr(null);
        setLoading(true);
        try {
            await login({ usernameOrEmail: v.usernameOrEmail, password: v.password });
        } catch {
            setErr("Login failed. Check credentials.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <Card style={{ width: 380 }}>
            <Typography.Title level={4} style={{ marginBottom: 16 }}>Login</Typography.Title>
            {err && <Alert type="error" message={err} style={{ marginBottom: 12 }} />}
            <Form layout="vertical" onFinish={onFinish} requiredMark={false}>
                <Form.Item name="usernameOrEmail" label="Username or Email" rules={[{ required: true }]}>
                    <Input autoComplete="username" />
                </Form.Item>
                <Form.Item name="password" label="Password" rules={[{ required: true }]}>
                    <Input.Password autoComplete="current-password" />
                </Form.Item>
                <Button type="primary" htmlType="submit" block loading={loading}>
                    Sign in
                </Button>
            </Form>
        </Card>
    );
}
