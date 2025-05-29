import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Input, Button, ColorPicker, Space } from "antd";
import { useStore } from "../stores/RootStore";
import { Color } from "antd/es/color-picker";

const TagCreationForm: React.FC = observer(() => {
    const [title, setTitle] = useState<string>("");
    const [color, setColor] = useState<Color>();

    const { tagStore } = useStore();

    const defaultColor = "ffffff";

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleColorChange = (color: Color) => {
        setColor(color);
    };

    const isCreateTagButtonDisabled = () => {
        return title.trim() === "";
    };

    const handleCreateTagButtonClick = async () => {
        await tagStore.createTag(title, color?.toHex() ?? defaultColor);
        setTitle("");
    };

    return (
        <Space direction="vertical">
            <strong>Новый тег</strong>
            <Input
                placeholder="Название тега"
                value={title}
                onChange={handleTitleChange}
                style={{ width: "300" }}
            />
            <ColorPicker
                value={color}
                defaultValue={defaultColor}
                onChange={handleColorChange}
            />
            <Button
                onClick={handleCreateTagButtonClick}
                disabled={isCreateTagButtonDisabled()}>
                Добавить
            </Button>
        </Space>
    );
});

export default TagCreationForm;
