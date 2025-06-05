import React from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Button, Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import ITask from "../../interfaces/ITask";
import TaskTag from "../tag/TaskTag";

interface ITaskInTrashListItemProps {
    task: ITask;
}

const TaskInTrashListItem: React.FC<ITaskInTrashListItemProps> = observer(
    props => {
        const { taskStore, tagStore } = useStore();

        const { task } = props;

        const isCompleted = task.completedDateTime !== null;

        const backgroundColor =
            taskStore.currentTask?.id === task.id ? "lightgray" : "";

        const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

        const taskTagComponents = taskTags
            ? taskTags.map(t => (
                  <TaskTag
                      key={t.id}
                      tag={t}
                  />
              ))
            : [];

        const handleTaskClick = () => {
            taskStore.setCurrentTask(task);
        };

        const handleRestoreTaskButtonClick = async () => {
            await taskStore.moveTaskFromTrash(task);
        };

        const handleDeleteTaskButtonClick = async () => {
            await taskStore.deleteTask(task);
        };

        return (
            <div
                onClick={handleTaskClick}
                style={{
                    color: "grey",
                    backgroundColor: backgroundColor,
                }}>
                <Checkbox
                    checked={isCompleted}
                    disabled={true}
                    style={{ marginRight: 5 }}
                />
                {task.title}
                <br />
                {taskTagComponents}
                {taskTagComponents.length > 0 ? <br /> : null}
                <Button onClick={handleRestoreTaskButtonClick}>
                    Восстановить
                </Button>
                <Button onClick={handleDeleteTaskButtonClick}>Удалить</Button>
                <Divider size="small" />
            </div>
        );
    }
);

export default TaskInTrashListItem;
