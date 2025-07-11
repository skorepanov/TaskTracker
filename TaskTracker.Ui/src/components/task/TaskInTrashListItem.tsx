import React from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Divider, Dropdown, MenuProps } from "antd";
import { useStore } from "../../stores/RootStore";
import { useSettings } from "../../contexts/SettingsContext";
import { useTranslation } from "../../hooks/useTranslation";
import { formatDate } from "../../utils/dateTimeFormatter";
import ITask from "../../interfaces/ITask";
import TaskTag from "../tag/TaskTag";

type MenuItem = Required<MenuProps>["items"][number];

interface ITaskInTrashListItemProps {
    task: ITask;
}

const TaskInTrashListItem: React.FC<ITaskInTrashListItemProps> = observer(
    props => {
        const { taskStore, folderStore, tagStore } = useStore();
        const { settings } = useSettings();
        const t = useTranslation();

        const { task } = props;

        const isCompleted = task.completedDateTime !== null;

        const selectedTaskColor =
            settings.theme === "dark" ? "#111a2c" : "#e6f4ff";
        const backgroundColor =
            taskStore.currentTask?.id === task.id ? selectedTaskColor : "";

        const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

        const taskTagComponents = taskTags
            ? taskTags.map(t => (
                  <TaskTag
                      key={t.id}
                      tag={t}
                  />
              ))
            : [];

        const dateComponent =
            task.completedDateTime !== null ? (
                formatDate(task.completedDateTime)
            ) : task.dueDateTime !== null ? (
                <span style={{ color: "red" }}>
                    {formatDate(task.dueDateTime)}
                </span>
            ) : null;

        const handleTaskClick = () => {
            taskStore.currentTaskId = task.id;
        };

        const handleRestoreTaskButtonClick = async (
            folderId: number | null
        ) => {
            await taskStore.moveTaskFromTrash(task.id, folderId);
        };

        const handleDeleteTaskButtonClick = async () => {
            await taskStore.deleteTask(task.id);
        };

        const inboxMenuItems: MenuItem[] = [
            {
                key: "restoreToInbox",
                label: "<Inbox>",
                onClick: async () => {
                    await handleRestoreTaskButtonClick(null);
                },
            },
            {
                type: "divider",
            },
        ];

        const folderMenuItems: MenuItem[] = folderStore
            .getSortedFolders()
            .map(f => {
                return {
                    key: `restoreTo${f.id}`,
                    label: f.title,
                    onClick: async () => {
                        await handleRestoreTaskButtonClick(f.id);
                    },
                };
            });

        const contextMenuItems: MenuItem[] = [
            {
                key: "restore",
                label: t("restoreTask"),
                children: [...inboxMenuItems, ...folderMenuItems],
            },
            {
                key: "delete",
                label: t("deleteTask"),
                onClick: handleDeleteTaskButtonClick,
            },
        ];

        return (
            <>
                <Dropdown
                    key={task.id}
                    menu={{ items: contextMenuItems }}
                    trigger={["contextMenu"]}>
                    <div
                        onClick={handleTaskClick}
                        style={{
                            color: "grey",
                            backgroundColor: backgroundColor,
                            paddingLeft: 10,
                            paddingTop: 10,
                            paddingBottom: 10,
                        }}>
                        <div style={{ float: "left" }}>
                            <Checkbox
                                checked={isCompleted}
                                disabled={true}
                                style={{ marginRight: 5 }}
                            />
                            {task.title}
                        </div>
                        <div style={{ float: "right" }}>
                            {taskTagComponents}
                        </div>
                        <br />
                        {dateComponent}
                    </div>
                </Dropdown>
                <Divider style={{ marginTop: 0, marginBottom: 0 }} />
            </>
        );
    }
);

export default TaskInTrashListItem;
