import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { DatePicker, Input, Select, Space } from "antd";
import { useStore } from "../../stores/RootStore";
import dayjs, { Dayjs } from "dayjs";

interface ITaskCreationPanelProps {
    dueDateTime?: Date;
    folderId?: number;
}

const TaskCreationPanel: React.FC<ITaskCreationPanelProps> = observer(props => {
    const inboxId = -1;

    const [title, setTitle] = useState<string>("");
    const [dueDateTime, setDueDateTime] = useState<Date | null>(
        props.dueDateTime ?? null
    );
    const [folderId, setFolderId] = useState<number>(props.folderId ?? inboxId);

    const { taskStore, folderStore } = useStore();

    const inboxOption = {
        key: inboxId,
        value: inboxId,
        label: "<Inbox>",
    };

    const folderOptions = folderStore.getSortedFolders().map(f => ({
        key: f.id,
        value: f.id,
        label: f.title,
    }));

    const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(event.target.value);
    };

    const handleDueDateTimeChange = (date: Dayjs | null) => {
        setDueDateTime(date?.toDate() ?? null);
    };

    const handleFolderChange = (folderId: number) => {
        setFolderId(folderId);
    };

    const handleTitlePressEnter = async () => {
        if (title.trim() === "") {
            return;
        }

        const normalizedFolderId = folderId === inboxId ? null : folderId;

        await taskStore.createTask(title, dueDateTime, normalizedFolderId);

        setTitle("");
        setDueDateTime(props.dueDateTime ?? null);
        setFolderId(props.folderId ?? inboxId);
    };

    return (
        <Space.Compact style={{ width: "100%" }}>
            <Input
                placeholder="Создать задачу"
                value={title}
                onChange={handleTitleChange}
                onPressEnter={handleTitlePressEnter}
                addonAfter={
                    <>
                        <Select
                            placeholder="Папка"
                            options={[inboxOption, ...folderOptions]}
                            value={folderId}
                            onChange={handleFolderChange}
                            showSearch
                            optionFilterProp="label"
                            popupMatchSelectWidth={false}
                            style={{ maxWidth: 250 }}
                        />
                        <DatePicker
                            placeholder="Дата"
                            value={
                                dueDateTime !== null ? dayjs(dueDateTime) : null
                            }
                            onChange={handleDueDateTimeChange}
                            style={{
                                width: 120,
                                marginLeft: 10,
                            }}
                        />
                    </>
                }
                style={{ marginTop: 10, marginBottom: 20, marginRight: 5 }}
            />
        </Space.Compact>
    );
});

export default TaskCreationPanel;
