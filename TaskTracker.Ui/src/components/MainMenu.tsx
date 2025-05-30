import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Button, Dropdown, Menu, MenuProps } from "antd";
import { useStore } from "../stores/RootStore";
import FolderCreationModal from "./FolderCreationModal";

type MenuItem = Required<MenuProps>["items"][number];

const MainMenu: React.FC = observer(() => {
    const [isFolderCreationModalOpen, setIsFolderCreationModalOpen] =
        React.useState(false);

    const location = useLocation();

    const { taskStore, folderStore } = useStore();

    const allIncompletedTaskCount = taskStore.incompletedTasks.length;

    const todayIncompletedTaskCount =
        taskStore.getTodayIncompletedTasks().length;

    const inboxIncompletedTasksCount =
        taskStore.getInboxIncompletedTasks().length;

    const getMenuItem = (
        link: string,
        title: string,
        incompletedTaskCount?: number
    ) => {
        return (
            <Link to={link}>
                <div style={{ float: "left" }}>{title}</div>
                <div style={{ float: "right", color: "grey" }}>
                    {incompletedTaskCount}
                </div>
            </Link>
        );
    };

    const handleCreateFolderButtonClick = () => {
        setIsFolderCreationModalOpen(true);
    };

    const hideFolderCreationModal = () => {
        setIsFolderCreationModalOpen(false);
    };

    const getFolderMenuItem = () => {
        return (
            <>
                <div style={{ float: "left" }}>Папки</div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ float: "right" }}>
                    <Button onClick={handleCreateFolderButtonClick}>+</Button>
                    <FolderCreationModal
                        isModalOpen={isFolderCreationModalOpen}
                        hideModal={hideFolderCreationModal}
                    />
                </div>
            </>
        );
    };

    const getFolderMenuItems = () => {
        return folderStore.folders.map(f => {
            const folderIncompleteTaskCount = taskStore.incompletedTasks.filter(
                t => t.folderId === f.id
            ).length;

            const handleDeleteFolderButtonClick = async () => {
                await folderStore.deleteFolder(f);
            };

            const contextMenu = {
                items: [
                    {
                        label: "Удалить папку",
                        key: "deleteFolder",
                    },
                ],
                onClick: handleDeleteFolderButtonClick,
            };

            return {
                key: `/folders/${f.id}`,
                label: (
                    <Dropdown
                        key={f.id}
                        menu={contextMenu}
                        trigger={["contextMenu"]}>
                        {getMenuItem(
                            `/folders/${f.id}`,
                            f.title,
                            folderIncompleteTaskCount
                        )}
                    </Dropdown>
                ),
            };
        });
    };

    const mainMenuItems: MenuItem[] = [
        {
            key: "/",
            label: getMenuItem("/", "Главная"),
        },
        {
            key: "/all",
            label: getMenuItem("/all", "Все задачи", allIncompletedTaskCount),
        },
        {
            key: "/today",
            label: getMenuItem("/today", "Сегодня", todayIncompletedTaskCount),
        },
        {
            key: "/inbox",
            label: getMenuItem("/inbox", "Inbox", inboxIncompletedTasksCount),
        },
        {
            type: "divider",
        },
        {
            key: "/folders",
            label: getFolderMenuItem(),
            children: getFolderMenuItems(),
        },
        {
            type: "divider",
        },
        {
            key: "/trash",
            label: getMenuItem("/trash", "Корзина"),
        },
    ];

    return (
        <Menu
            items={mainMenuItems}
            mode="inline"
            selectedKeys={[location.pathname]}
            defaultOpenKeys={["/folders"]}
            style={{ width: "270px" }}
        />
    );
});

export default MainMenu;
